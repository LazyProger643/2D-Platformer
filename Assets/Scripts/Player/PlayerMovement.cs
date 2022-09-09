using UnityEngine;

public class PlayerMovement : PhysicsMovement
{
    protected override bool IsPlatform(Collider2D collider)
    {
        return collider.HasComponent<Platform>();
    }
}
