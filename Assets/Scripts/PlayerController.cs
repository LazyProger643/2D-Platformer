using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Min(1)] private float _movingSpeed = 1;
    [SerializeField, Min(1)] private float _jumpForce = 12;
    [SerializeField] private CircleCollider2D _groundCollider;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundLayer;

    private readonly float _groundDetectionDistance = 0.015f;
    private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];

    private ContactFilter2D _groundFilter;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private bool _isJumping = false;
    private bool _isGrounded = false;
    private float _currentMovingSpeed = 0;
    private float _minVelocity = 0.001f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _groundFilter = new ContactFilter2D();
        _groundFilter.SetLayerMask(_groundLayer);
    }

    private void FixedUpdate()
    {
        UpdateState();
        HandleInput();
        UpdateCamera();
        UpdateAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Gem gem))
        {
            float delayBeforeDestroy = 0;

            if (collision.TryGetComponent(out Animator animator))
            {
                animator.SetTrigger("Pickup");
                delayBeforeDestroy = 0.3f;
            }

            Destroy(gem.gameObject, delayBeforeDestroy);
        }
    }

    private void UpdateState()
    {
        int hitCount = _groundCollider.Cast(Vector2.down, _groundFilter, _hits, _groundDetectionDistance);
        int hitCount2 = _groundCollider.Cast(Vector2.up, _groundFilter, _hits, _groundDetectionDistance);

        _isGrounded = hitCount > 0 && hitCount2 == 0;

        if (_isJumping && _rigidbody2D.velocity.y < _minVelocity)
        {
            _isJumping = false;
        }
    }

    private void UpdateCamera()
    {
        _camera.transform.position = new Vector3(transform.position.x, transform.position.y, _camera.transform.position.z);
    }

    private void UpdateAnimation()
    {
        bool isFalling = _isGrounded == false && _rigidbody2D.velocity.y < -_minVelocity;
        bool isMooving = _isGrounded && _currentMovingSpeed != 0 && _isJumping == false && isFalling == false;

        _animator.SetBool("Run", isMooving);
        _animator.SetBool("Idle", _isGrounded && isMooving == false && _isJumping == false && isFalling == false);
        _animator.SetBool("Jumping", _isJumping);
        _animator.SetBool("Falling", isFalling);

        UpdateSpriteOrientation();
    }

    private bool CanJump()
    {
        return _isJumping == false && _isGrounded;
    }

    private void HandleInput()
    {
        if (Input.GetAxis("Jump") > 0 && CanJump())
        {
            Jump();
        }

        _currentMovingSpeed = Input.GetAxis("Horizontal") * _movingSpeed;
        _rigidbody2D.velocity = new Vector2(_currentMovingSpeed, _rigidbody2D.velocity.y);
    }

    private void Jump()
    {
        _isJumping = true;
        _rigidbody2D.AddForce(Vector2.up * (_jumpForce - _rigidbody2D.velocity.y), ForceMode2D.Impulse);
    }

    private void UpdateSpriteOrientation()
    {
        if (_rigidbody2D.velocity.x != 0)
        {
            bool moveForward = _rigidbody2D.velocity.x > 0;

            if (_spriteRenderer.flipX == moveForward)
            {
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
            }
        }
    }
}
