using System.Collections.Generic;
using UnityEngine;

public class PointList : MonoBehaviour
{
    private List<Point> _points = new List<Point>();
    private int _currentPoint = 0;

    public int Count => _points.Count;

    private void Start()
    {
        foreach (Point point in GetComponentsInChildren<Point>())
        {
            _points.Add(point);
        }
    }

    public bool TryGetNextPoint(out Point point)
    {
        point = null;

        if (_points.Count == 0)
        {
            return false;
        }

        point = _points[_currentPoint];
        _currentPoint++;

        if (_currentPoint >= _points.Count)
        {
            _currentPoint = 0;
        }

        return true;
    }
}
