using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    private const string Running = "Running";
    private const string JumpTrigger = "Jump";
    private const string FallTrigger = "Fall";
    private const string LandedTrigger = "Landed";

    private SpriteRenderer _renderer;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void OnJumped()
    {
        _animator.SetTrigger(JumpTrigger);
    }

    public void OnBeganToFall()
    {
        _animator.SetTrigger(FallTrigger);
    }

    public void OnLanded()
    {
        _animator.SetTrigger(LandedTrigger);
    }

    public void OnMovementChanged(float horizontalMovement)
    {
        _animator.SetBool(Running, horizontalMovement != 0);

        FlipSpriteAlongDirection(horizontalMovement);
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
