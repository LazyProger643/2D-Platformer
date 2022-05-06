using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Gem : MonoBehaviour
{
    private readonly float _destroyDelay = 0.3f;
    private bool _isDestroyed = false;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Destroy()
    {
        if (_isDestroyed == false)
        {
            _animator.SetTrigger("Pickup");
            Destroy(gameObject, _destroyDelay);
        }

        _isDestroyed = true;
    }
}
