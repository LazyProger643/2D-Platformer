using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Range(1, 5)] private int _maxValue;
    [SerializeField] private UnityEvent<int> _healthChanged;

    private int _value;

    public int Value => _value;

    public void Reset()
    {
        _value = _maxValue;

        _healthChanged.Invoke(_value);
    }

    public void TakeDamage()
    {
        if (_value > 0)
        {
            _value--;

            _healthChanged.Invoke(_value);
        }
    }
}
