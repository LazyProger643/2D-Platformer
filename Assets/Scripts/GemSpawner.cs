using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private PointList _pointList;
    [SerializeField] private Gem _template;

    void Start()
    {
        for (int i = 0; i < _pointList.Count; i++)
        {
            if (_pointList.TryGetNextPoint(out Point point))
            {
                Instantiate(_template, point.transform.position, Quaternion.identity);
            }
        }
    }
}
