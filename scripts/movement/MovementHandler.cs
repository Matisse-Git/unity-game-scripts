using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionHandler))]
public class MovementHandler : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float axisX;
    public bool canMove;

    [Header("Turn Around")]
    public float turnAroundTime;
    public bool isTurningAround;

    DirectionHandler directionHandler;


    void Start()
    {
        directionHandler = GetComponent<DirectionHandler>();
    }

    void FixedUpdate()
    {
        if (canMove)
            MoveInDirection();
    }

    public void SetAxisX(float newAxisX)
    {
        axisX = newAxisX;
    }

    public void MoveInDirection()
    {
        transform.Translate(Vector2.right * axisX * (speed * Time.fixedDeltaTime));
    }

    public bool ShouldFaceRight(bool facingRight)
    {
        if (axisX < 0)
            return false;
        else if (axisX > 0)
            return true;

        return facingRight;
    }

    public IEnumerator TurnAround(float turnAroundTimeParam = -1f)
    {
        canMove = false;
        isTurningAround = true;

        if (turnAroundTimeParam != -1f)
            yield return new WaitForSeconds(turnAroundTimeParam);
        else
            yield return new WaitForSeconds(turnAroundTime);

        canMove = true;
        axisX = directionHandler.ChangeDirection(axisX);
        isTurningAround = false;
    }
}
