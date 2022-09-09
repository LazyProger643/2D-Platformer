using UnityEngine;

public class TargetFollowing : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    private void Update()
    {
        transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z);
    }
}
