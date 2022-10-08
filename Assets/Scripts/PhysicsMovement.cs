using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsMovement : MonoBehaviour
{
    [SerializeField, Range(1, 12)] private float _movingSpeed = 1;
    [SerializeField, Range(2, 20)] private float _jumpForce = 12;
    [SerializeField] private float _minGroundNormalY = 0.65f;
    [SerializeField] private float _gravityModifier = 1;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private UnityEvent Jumped;
    [SerializeField] private UnityEvent BeganToFall;
    [SerializeField] private UnityEvent Landed;
    [SerializeField] private UnityEvent<float> MovementChanged;

    private const float GroundStickingDistance = 0.5f;
    private const float MinMoveDistance = 0.001f;
    private const float ShellRadius = 0.01f;

    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];

    private ContactFilter2D _groundFilter;
    private Rigidbody2D _rigidbody;
    private Collider2D _bodyCollider;
    private Vector2 _velocity;
    private Vector2 _oldVelocity;
    private Vector2 _targetVelocity;
    private Vector2 _groundNormal;
    private bool _wasGrounded;
    private bool _isGrounded;
    private bool _isJumping;

    private void Awake()
    {
        _bodyCollider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _groundFilter = new ContactFilter2D();
        _groundFilter.SetLayerMask(_groundLayer);
        _groundFilter.useTriggers = false;
    }

    public void FixedUpdate()
    {
        Move();
        TryStickToGround();

        if (_wasGrounded == false && _isGrounded)
        {
            Landed.Invoke();
        }

        if (_isGrounded == false && _oldVelocity.y >= 0 && _velocity.y < 0)
        {
            _isJumping = false;

            BeganToFall.Invoke();
        }

        _oldVelocity = _velocity;
        _wasGrounded = _isGrounded;
    }

    public void OnMove(CallbackContext context)
    {
        _targetVelocity = context.ReadValue<Vector2>();
        _targetVelocity.y = 0;
        _targetVelocity = _targetVelocity.normalized * _movingSpeed;

        MovementChanged.Invoke(_targetVelocity.x);
    }

    public void OnJump(CallbackContext context)
    {
        if (context.performed && _isGrounded)
        {
            _velocity.y = _jumpForce;
            _isJumping = true;

            Jumped.Invoke();
        }
    }

    protected virtual bool IsPlatform(Collider2D collider)
    {
        return false;
    }

    private void Move()
    {
        _velocity += _gravityModifier * Time.deltaTime * Physics2D.gravity;
        _velocity.x = _targetVelocity.x;
        _isGrounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveVector;

        if (_wasGrounded)
        {
            Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
            moveVector = moveAlongGround * deltaPosition.x;
        }
        else
        {
            moveVector = deltaPosition * Time.deltaTime;
            moveVector.x = _targetVelocity.x * Time.deltaTime;
        }

        MoveByVector(moveVector, false);

        moveVector = Vector2.up * deltaPosition.y;

        MoveByVector(moveVector, true);
    }

    private void TryStickToGround()
    {
        if (_isJumping == false && _wasGrounded && _isGrounded == false)
        {
            int hitCount = _rigidbody.Cast(Vector2.down, _groundFilter, _hitBuffer, GroundStickingDistance + ShellRadius);

            if (hitCount > 0)
            {
                MoveByVector(Vector2.down, true);
            }
        }
    }

    private float GetColliderBottomY(Collider2D collider)
    {
        return collider.bounds.center.y - collider.bounds.extents.y;
    }

    private void MoveByVector(Vector2 moveVector, bool yMovement)
    {
        float distance = moveVector.magnitude;

        if (distance > MinMoveDistance)
        {
            int hitCount = _rigidbody.Cast(moveVector, _groundFilter, _hitBuffer, distance + ShellRadius);

            for (int i = 0; i < hitCount; i++)
            {
                if (IsPlatform(_hitBuffer[i].collider))
                {
                    if (GetColliderBottomY(_bodyCollider) < _hitBuffer[i].point.y)
                    {
                        continue;
                    }
                }

                Vector2 currentNormal = _hitBuffer[i].normal;

                if (currentNormal.y > _minGroundNormalY)
                {
                    _isGrounded = true;

                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);

                if (projection < 0)
                {
                    _velocity -= projection * currentNormal;
                }

                float modifiedDistance = _hitBuffer[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidbody.position += moveVector.normalized * distance;
    }
}
