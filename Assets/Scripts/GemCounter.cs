using UnityEngine;
using UnityEngine.Events;

public class GemCounter : MonoBehaviour
{
    [SerializeField] private UnityEvent<int> _gemCountChanged;

    private int _count;

    public void Reset()
    {
        _count = 0;
        _gemCountChanged?.Invoke(_count);
    }

    public void OnGemCollected()
    {
        _count++;
        _gemCountChanged?.Invoke(_count);
    }
}
