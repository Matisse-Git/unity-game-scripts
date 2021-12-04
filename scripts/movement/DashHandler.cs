using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionHandler), typeof(CollisionHandler))]
public class DashHandler : Rigidbody2DHandler
{
    [Header("Dash")]
    public float dashStrength;
    public float dashLength;
    public float dashDelay;
    public bool canDash;
    public bool isDashing;

    RigidbodyConstraints2D yFreeze = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    RigidbodyConstraints2D rotationFreeze = RigidbodyConstraints2D.FreezeRotation;

    DirectionHandler directionHandler;
    CollisionHandler collisionHandler;

    protected override void Start()
    {
        base.Start();
        directionHandler = GetComponent<DirectionHandler>();
        collisionHandler = GetComponent<CollisionHandler>();
    }
    
    public void Dash()
    {
        if (canDash)
        {
            canDash = false;
            isDashing = true;

            rb.constraints = yFreeze;

            ResetVerticalVelocity();
            ResetHorizontalVelocity();

            if (directionHandler.facingRight)
                rb.AddForce(Vector3.right * dashStrength, ForceMode2D.Impulse);
            else
                rb.AddForce(Vector3.left * dashStrength, ForceMode2D.Impulse);


            StartCoroutine(DashLength());
        }
    }

    IEnumerator DashLength()
    {
        yield return new WaitForSeconds(dashLength);

        isDashing = false;
        StopDash();
        StartCoroutine(DashDelay());
    }

    IEnumerator DashDelay()
    {
        yield return new WaitForSeconds(dashDelay);

        if (!collisionHandler.isGrounded)
            StartCoroutine(EnableDashWhenLanding());
        else
            canDash = true;
    }

    IEnumerator EnableDashWhenLanding()
    {
        while (!collisionHandler.isGrounded)
            yield return null;

        canDash = true;
    }

    void StopDash(){
        ResetHorizontalVelocity();
        ResetVerticalVelocity();
        rb.constraints = rotationFreeze;
    }
}
