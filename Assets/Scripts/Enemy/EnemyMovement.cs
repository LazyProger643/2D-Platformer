using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField, Min(1)] private float _moveSpeed = 1;
    [SerializeField] private PointList _waypoints;

    private const float GapToTarget = 0.1f;

    private float _movementDirection = 0;
    private int _currentPointIndex = 0;
    private Rigidbody2D _rigidbody2D;
    private Point _targetPoint;

    public float MovementDirection => _movementDirection;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _targetPoint = _waypoints.Points[_currentPointIndex];
    }

    public void FixedUpdate()
    {
        _movementDirection = _targetPoint.transform.position.x - transform.position.x;
        float movementDistance = Math.Abs(_movementDirection);

        if (_movementDirection != 0)
        {
            _movementDirection /= movementDistance;
        }

        if (movementDistance < GapToTarget)
        {
            SetNextTargetPoint();
        }

        _rigidbody2D.velocity = new Vector2(_movementDirection * _moveSpeed, _rigidbody2D.velocity.y);
    }

    private void SetNextTargetPoint()
    {
        _currentPointIndex = (_currentPointIndex + 1) % _waypoints.Points.Length;
        _targetPoint = _waypoints.Points[_currentPointIndex];
    }
}
