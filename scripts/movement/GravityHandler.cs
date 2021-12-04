using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHandler : Rigidbody2DHandler
{
    [Header("Gravity")]
    public bool isJumping;
    public bool isFalling;
    public GravityData gravityData;

    public void Update()
    {
        isJumping = CheckJumping();
        isFalling = CheckFalling();
    }

    void LimitVerticalVelocity()
    {
        rb.velocity = new Vector2(rb.velocity.x, gravityData.maxFallingVelocity);
    }

    bool CheckFalling()
    {
        if (rb.velocity.y < 0)
            return true;
        else
            return false;
    }

    bool CheckJumping()
    {
        if (rb.velocity.y > 0)
            return true;
        else
            return false;
    }
}
