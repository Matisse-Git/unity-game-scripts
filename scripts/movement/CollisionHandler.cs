using UnityEngine;

public class CollisionHandler : Rigidbody2DHandler
{
    [Header("Collision")]
    public bool isGrounded;
    public bool wallLeft;
    public bool wallRight;

    public string CheckWallCollisionEnter(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.name == "leftSideCollider")
        {
            ResetHorizontalVelocity();
            return "left";
        }
        if (collision.otherCollider.gameObject.name == "rightSideCollider")
        {
            ResetHorizontalVelocity();
            return "right";
        }

        return "none";
    }

    public string CheckWallCollisionExit(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.name == "leftSideCollider")
        {
            return "left";
        }
        if (collision.otherCollider.gameObject.name == "rightSideCollider")
        {
            return "right";
        }

        return "none";
    }

    public bool CheckGroundCollisionEnter(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Foreground"))
        {
            if (collision.otherCollider.gameObject.name == "groundCollider")
                return true;
        }

        return false;
    }

    public bool CheckGroundCollisionExit(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Foreground"))
        {
            if (collision.otherCollider.gameObject.name == "groundCollider")
                return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckGroundCollisionEnter(collision))
        {
            isGrounded = true;
            ResetVerticalVelocity();
            ResetHorizontalVelocity();
        }

        switch (CheckWallCollisionEnter(collision))
        {
            case "left":
                wallLeft = true;
                break;
            case "right":
                wallRight = true;
                break;
            default:
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (CheckGroundCollisionExit(collision))
            isGrounded = false;

        switch (CheckWallCollisionExit(collision))
        {
            case "left":
                wallLeft = false;
                break;
            case "right":
                wallRight = false;
                break;
            default:
                break;
        }
    }


}
