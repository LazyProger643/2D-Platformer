using UnityEngine;

public class PointList : MonoBehaviour
{
    private Point[] _points;

    public Point[] Points
    {
        get
        {
            if (_points is null)
            {
                _points = GetComponentsInChildren<Point>();
            }

            return _points;
        }
    }

    private void OnDrawGizmos()
    {
        if (Points.Length > 1)
        {
            for (int i = 1; i < Points.Length; i++)
            {
                Vector3 from = Points[i - 1].transform.position;
                Vector3 to = Points[i].transform.position;

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(from, to);
            }
        }
    }
}
