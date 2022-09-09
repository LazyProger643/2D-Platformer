using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] private SpriteEffect _itemPickUp;
    [SerializeField] private SpriteEffect _playerDeath;

    public void SpawnItemPickUp(Vector3 position)
    {
        Instantiate(_itemPickUp, position, Quaternion.identity, gameObject.transform);
    }

    public void SpawnPlayerDeath(Vector3 position)
    {
        Instantiate(_playerDeath, position, Quaternion.identity, gameObject.transform);
    }
}
