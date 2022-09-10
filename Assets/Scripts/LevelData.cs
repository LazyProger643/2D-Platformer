using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private Transform _playerStartPosition;
    [SerializeField] private GemContainer _gems;

    public Vector3 PlayerStartPosition => _playerStartPosition.position;
    public int GemCount => _gems.GetComponentsInChildren<Gem>().Length;
}
