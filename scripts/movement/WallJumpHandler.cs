using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionHandler), typeof(GravityHandler), typeof(DirectionHandler))]
[RequireComponent(typeof(MovementHandler))]
public class WallJumpHandler : Rigidbody2DHandler
{
    [Header("Wall Jump")]
    public Vector2 wallJumpStrength;
    public float wallJumpLength;
    public bool canWallJump;

    [Header("Wall Cling")]
    public float wallClingGravity;
    public float maxWallClingVelocity;
    bool shouldLimitWallClingVelocity;
    public float leaveWallGracePeriod;
    public bool isWallClinging;
    float leaveWallProgress;

    CollisionHandler collisionHandler;
    GravityHandler gravityHandler;
    DirectionHandler directionHandler;
    MovementHandler movementHandler;

    protected override void Start()
    {
        base.Start();
        collisionHandler = GetComponent<CollisionHandler>();
        gravityHandler = GetComponent<GravityHandler>();
        directionHandler = GetComponent<DirectionHandler>();
        movementHandler = GetComponent<MovementHandler>();
    }

    void Update()
    {
        if (!isWallClinging && (collisionHandler.wallLeft || collisionHandler.wallRight) && gravityHandler.isFalling)
        {
            ClingToWall();
            StartCoroutine(LeaveWallGracePeriod());
        }
        else if (isWallClinging && (!collisionHandler.wallLeft && !collisionHandler.wallRight) || collisionHandler.isGrounded)
        {
            StopClingingToWall();
            StopCoroutine(LeaveWallGracePeriod());
            movementHandler.canMove = true;
        }

        if (isWallClinging && shouldLimitWallClingVelocity)
            LimitWallClingVelocity(maxWallClingVelocity);
    }

    public void WallJump()
    {
        if (canWallJump)
        {
            canWallJump = false;
            shouldLimitWallClingVelocity = false;

            var rightForce = new Vector2(Vector2.right.x * wallJumpStrength.x, Vector2.up.y * wallJumpStrength.y);
            var leftForce = new Vector2(Vector2.left.x * wallJumpStrength.x, Vector2.up.y * wallJumpStrength.y);

            if (collisionHandler.wallLeft)
                rb.AddForce(rightForce, ForceMode2D.Impulse);
            else if (collisionHandler.wallRight)
                rb.AddForce(leftForce, ForceMode2D.Impulse);

            StartCoroutine(WallJumpLength());
        }
    }

    IEnumerator WallJumpLength()
    {
        yield return new WaitForSeconds(wallJumpLength);

        ResetHorizontalVelocity();
        shouldLimitWallClingVelocity = true;
    }

    void ClingToWall()
    {
        canWallJump = true;
        isWallClinging = true;
        movementHandler.canMove = false;

        rb.gravityScale = wallClingGravity;

        if (collisionHandler.wallLeft)
            directionHandler.facingRight = true;
        else if (collisionHandler.wallRight)
            directionHandler.facingRight = false;
    }

    void StopClingingToWall()
    {
        canWallJump = false;
        isWallClinging = false;
        movementHandler.canMove = true;

        rb.gravityScale = gravityHandler.gravityData.normalGravity;
    }


    public void LimitWallClingVelocity(float maxWallClingVelocity)
    {
        rb.velocity = new Vector2(rb.velocity.x, maxWallClingVelocity);
    }


    IEnumerator LeaveWallGracePeriod()
    {
        while (leaveWallProgress < leaveWallGracePeriod)
        {
            if ((collisionHandler.wallLeft && movementHandler.axisX > 0) || (collisionHandler.wallRight && movementHandler.axisX < 0))
                leaveWallProgress += Time.deltaTime;
            else
                leaveWallProgress = 0.0f;

            yield return null;
        }

        leaveWallProgress = 0.0f;
        movementHandler.canMove = true;
    }
}
