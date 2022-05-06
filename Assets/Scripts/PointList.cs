using UnityEngine;

public class PointList : MonoBehaviour
{
    private Point[] _points;
    private int _currentPoint = 0;

    public int Count => _points.Length;

    private void Awake()
    {
        _points = GetComponentsInChildren<Point>();
    }

    public Point GetNextPoint()
    {
        if (_points.Length == 0)
        {
            return null;
        }

        if (_currentPoint >= _points.Length)
        {
            _currentPoint = 0;
        }

        Point point = _points[_currentPoint];

        _currentPoint++;

        return point;
    }

    public bool TryGetNextPoint(out Point point)
    {
        point = GetNextPoint();

        return point != null;
    }
}
