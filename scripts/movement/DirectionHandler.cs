using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionHandler : MonoBehaviour
{
    [Header("Direction")]
    public bool facingRight;
    bool currentDirection;
    bool lastDirection;
    public bool canChangeDirection;

    MovementHandler movementHandler;

    void Start()
    {
        movementHandler = GetComponent<MovementHandler>();
    }

    void Update()
    {
        facingRight = movementHandler.ShouldFaceRight(facingRight);
    }

    public float ChangeDirection(float axisX)
    {
        return -axisX;
    }
}
