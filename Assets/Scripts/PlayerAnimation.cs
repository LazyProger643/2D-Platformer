using UnityEngine;

public class PlayerAnimation
{
    public static string RunTrigger = "Run";
    public static string IdleTrigger = "Idle";
    public static string JumpTrigger = "Jump";
    public static string FallTrigger = "Fall";

    private readonly SpriteRenderer _renderer;
    private readonly Animator _animator;

    public PlayerAnimation(Animator animator, SpriteRenderer renderer)
    {
        _animator = animator;
        _renderer = renderer;
    }

    public void TriggerAnimation(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    public void StopAnimation(string trigger)
    {
        _animator.ResetTrigger(trigger);
    }

    public void UpdateOrientation(float horizontalDirection)
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
