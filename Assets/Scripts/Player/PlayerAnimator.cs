using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private const string RunTrigger = "Run";
    private const string IdleTrigger = "Idle";
    private const string JumpTrigger = "Jump";
    private const string FallTrigger = "Fall";

    private SpriteRenderer _renderer;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void OnJumped()
    {
        TriggerAnimation(JumpTrigger);
    }

    public void OnBeganToFall()
    {
        TriggerAnimation(FallTrigger);
    }

    public void OnLanded(float horizontalMovement)
    {
        SetGroundAnimation(horizontalMovement);
    }

    public void OnMovementChanged(float horizontalMovement, bool isGrounded)
    {
        FlipSpriteAlongDirection(horizontalMovement);

        if (isGrounded)
        {
            SetGroundAnimation(horizontalMovement);
        }
    }

    private void SetGroundAnimation(float horizontalMovement)
    {
        if (horizontalMovement == 0)
        {
            TriggerAnimation(IdleTrigger);
        }
        else
        {
            TriggerAnimation(RunTrigger);
        }
    }

    private void TriggerAnimation(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    private void FlipSpriteAlongDirection(float horizontalDirection)
    {
        if (horizontalDirection != 0)
        {
            bool orientToRight = horizontalDirection > 0;

            if (_renderer.flipX == orientToRight)
            {
                _renderer.flipX = !_renderer.flipX;
            }
        }
    }
}
