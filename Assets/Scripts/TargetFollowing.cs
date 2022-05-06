using UnityEngine;

public class TargetFollowing : MonoBehaviour
{
    [SerializeField] private Transform _target;

    void Update()
    {
        transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
    }
}
