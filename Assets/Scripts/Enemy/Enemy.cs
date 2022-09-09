using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    private EnemyMovement _movement;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        float movementDirection = _movement.MovementDirection;

        if (movementDirection != 0)
        {
            FlipSpriteAlongDirection(movementDirection > 0);
        }
    }

    private void FlipSpriteAlongDirection(bool moveForward)
    {
        if (_spriteRenderer.flipX != moveForward)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
    }
}
