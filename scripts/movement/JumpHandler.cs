using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionHandler), typeof(GravityHandler))]
public class JumpHandler : Rigidbody2DHandler
{
    [Header("Jump")]
    public float jumpStrength;
    public float jumpLength;
    public float jumpDelay;
    public bool canJump;

    CollisionHandler collisionHandler;
    GravityHandler gravityHandler;

    protected override void Start()
    {
        base.Start();
        collisionHandler = GetComponent<CollisionHandler>();
        gravityHandler = GetComponent<GravityHandler>();
    }

    public void Jump()
    {
        if (canJump)
        {
            canJump = false;

            rb.velocity += Vector2.up * jumpStrength;
            StartCoroutine(JumpDelay());
        }

    }

    IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(jumpDelay);

        if (!collisionHandler.isGrounded)
            StartCoroutine(EnableJumpWhenLanding());
        else
            canJump = true;

    }

    IEnumerator EnableJumpWhenLanding()
    {
        while (!collisionHandler.isGrounded)
            yield return null;

        canJump = true;
    }

    IEnumerator JumpWhenLanding()
    {
        while (!collisionHandler.isGrounded)
            yield return null;

        Jump();
    }

    public void CancelJump()
    {
        if (gravityHandler.isJumping)
            ResetVerticalVelocity();
    }
}
