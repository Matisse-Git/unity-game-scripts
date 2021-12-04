using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rigidbody2DHandler : MonoBehaviour
{
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void ResetHorizontalVelocity()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    protected void ResetVerticalVelocity()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }
}
