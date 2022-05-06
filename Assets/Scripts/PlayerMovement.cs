using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(1)] private float _movingSpeed = 1;
    [SerializeField, Min(1)] private float _jumpForce = 12;
    [SerializeField] private LayerMask _groundLayer;

    private readonly float _groundDetectionDistance = 0.015f;
    private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];
    private readonly Dictionary<PlayerState, string> _stateAnimationTriggers = new Dictionary<PlayerState, string> {
        {PlayerState.Idling, PlayerAnimation.IdleTrigger},
        {PlayerState.Running, PlayerAnimation.RunTrigger},
        {PlayerState.Jumping, PlayerAnimation.JumpTrigger},
        {PlayerState.Falling, PlayerAnimation.FallTrigger}
    };

    private ContactFilter2D _groundFilter;
    private PlayerAnimation _animation;
    private Rigidbody2D _rigidbody;
    private PlayerState _state;

    private bool _isGrounded;
    private bool _wantToJump;

    private void Start()
    {
        _animation = new PlayerAnimation(GetComponent<Animator>(), GetComponent<SpriteRenderer>());
        _rigidbody = GetComponent<Rigidbody2D>();

        _groundFilter = new ContactFilter2D();
        _groundFilter.SetLayerMask(_groundLayer);

        SetState(PlayerState.Idling);
    }

    private void Update()
    {
        _rigidbody.velocity = new Vector2(Input.GetAxis("Horizontal") * _movingSpeed, _rigidbody.velocity.y);

        if (Input.GetAxis("Jump") > 0)
        {
            _wantToJump = true;
        }
    }

    private void FixedUpdate()
    {
        int downHitCount = _rigidbody.Cast(Vector2.down, _groundFilter, _hits, _groundDetectionDistance);
        int upHitCount = _rigidbody.Cast(Vector2.up, _groundFilter, _hits, _groundDetectionDistance);

        _isGrounded = downHitCount > 0 && upHitCount == 0;

        StateUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Gem gem))
        {
            gem.Destroy();
        }
    }

    private void SetState(PlayerState state)
    {
        _animation.TriggerAnimation(_stateAnimationTriggers[state]);
        _animation.UpdateOrientation(_rigidbody.velocity.x);

        if (state == PlayerState.Jumping)
        {
            _rigidbody.AddForce(Vector2.up * (_jumpForce - _rigidbody.velocity.y), ForceMode2D.Impulse);
        }

        _wantToJump = false;
        _state = state;
    }

    private void StateUpdate()
    {
        switch (_state)
        {
            case PlayerState.Idling:
                IdlingStateUpdate();
                break;
            case PlayerState.Running:
                RunningStateUpdate();
                break;
            case PlayerState.Jumping:
                JumpingStateUpdate();
                break;
            case PlayerState.Falling:
                FallingStateUpdate();
                break;
            default:
                break;
        }

        if (_rigidbody.velocity.x != 0)
        {
            _animation.UpdateOrientation(_rigidbody.velocity.x);
        }

        _wantToJump = false;
    }

    private void IdlingStateUpdate()
    {
        if (_wantToJump)
        {
            SetState(PlayerState.Jumping);
        }
        else if (_rigidbody.velocity.x != 0)
        {
            SetState(PlayerState.Running);
        }
        else if (_isGrounded == false)
        {
            SetState(PlayerState.Falling);
        }
    }

    private void RunningStateUpdate()
    {
        if (_wantToJump)
        {
            SetState(PlayerState.Jumping);
        }
        else if (_isGrounded == false)
        {
            SetState(PlayerState.Falling);
        }
        else if (_rigidbody.velocity.x == 0)
        {
            SetState(PlayerState.Idling);
        }
    }

    private void JumpingStateUpdate()
    {
        if (_rigidbody.velocity.y < 0)
        {
            SetState(PlayerState.Falling);
        }
    }

    private void FallingStateUpdate()
    {
        if (_isGrounded)
        {
            if (_wantToJump)
            {
                SetState(PlayerState.Jumping);
            }
            else if (_rigidbody.velocity.x == 0)
            {
                SetState(PlayerState.Idling);
            }
            else
            {
                SetState(PlayerState.Running);
            }
        }
    }
}
