using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private PointList _patrolPoints;
    [SerializeField, Min(1)] private float _moveSpeed = 1;

    private readonly float _gapToTarget = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Point _targetPoint;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        SetNextTargetPoint();
    }

    private void FixedUpdate()
    {
        if (_targetPoint != null)
        {
            Vector2 targetPosition = new Vector2(_targetPoint.transform.position.x, 0);
            Vector2 currentPosition = new Vector2(transform.position.x, 0);
            Vector2 force = (targetPosition - currentPosition).normalized * _moveSpeed;

            _rigidbody2D.velocity = new Vector2(force.x, _rigidbody2D.velocity.y);

            if ((targetPosition - currentPosition).magnitude < _gapToTarget)
            {
                SetNextTargetPoint();
            }
        }

        UpdateSpriteOrientation();
    }

    private void SetNextTargetPoint()
    {
        _patrolPoints.TryGetNextPoint(out _targetPoint);
    }

    private void UpdateSpriteOrientation()
    {
        if (_rigidbody2D.velocity.x != 0)
        {
            bool moveForward = _rigidbody2D.velocity.x > 0;

            if (_spriteRenderer.flipX != moveForward)
            {
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
            }
        }
    }
}
