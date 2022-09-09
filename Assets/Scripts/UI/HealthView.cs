using System.Collections.Generic;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Heart _heartPrefab;

    private readonly List<Heart> _hearts = new List<Heart>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnHealthChanged(int value)
    {
        if (value > _hearts.Count)
        {
            int heartCount = value - _hearts.Count;

            for (int i = 0; i < heartCount; i++)
            {
                CreateHeart();
            }
        }
        else
        {
            int heartCount = _hearts.Count - value;

            for (int i = 0; i < heartCount; i++)
            {
                DeleteHeart();
            }
        }
    }

    private void CreateHeart()
    {
        Heart heart = Instantiate(_heartPrefab, transform);
        _hearts.Add(heart);
    }

    private void DeleteHeart()
    {
        Destroy(_hearts[_hearts.Count - 1].gameObject);
        _hearts.RemoveAt(_hearts.Count - 1);
    }
}
