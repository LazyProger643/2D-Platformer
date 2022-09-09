using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerInvulnerability))]
[RequireComponent(typeof(PlayerTakingDamageEffect))]
public class Player : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector3> GemCollected;
    [SerializeField] private UnityEvent<Vector3> Died;

    private bool _isEnabled;
    private PlayerInput _input;
    private PlayerHealth _health;
    private PlayerInvulnerability _invulnerability;
    private PlayerTakingDamageEffect _takingDamageEffect;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _health = GetComponent<PlayerHealth>();
        _invulnerability = GetComponent<PlayerInvulnerability>();
        _takingDamageEffect = GetComponent<PlayerTakingDamageEffect>();

        _health.Reset();
        Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    public void Activate()
    {
        _isEnabled = true;
        _input.ActivateInput();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _isEnabled = false;
        _input.DeactivateInput();
    }

    public void OnLevelLoaded()
    {
        _health.Reset();
    }

    private void HandleCollision(Collider2D collision)
    {
        if (gameObject.activeSelf && _isEnabled)
        {
            if (collision.TryGetComponent(out Gem gem))
            {
                CollectGem(gem);
            }

            if (_invulnerability.IsActive == false && collision.HasComponent<Enemy>())
            {
                TakeDamage();
            }
        }
    }

    private void CollectGem(Gem gem)
    {
        GemCollected.Invoke(gem.transform.position);

        Destroy(gem.gameObject);
    }

    private void TakeDamage()
    {
        _health.TakeDamage();

        if (_health.Value > 0)
        {
            _takingDamageEffect.Blink();
            _invulnerability.Activate();
        }
        else
        {
            gameObject.SetActive(false);
            Died.Invoke(transform.position);
        }
    }
}
