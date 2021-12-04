using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementHandler), typeof(CollisionHandler), typeof(DirectionHandler))]
public class EdgeDetectionHandler : MonoBehaviour
{
    [Header("Edge Detection")]
    public Vector2 detectionSize;
    public Vector2 detectionOffset;
    Vector2 currentDetectionOffset;
    public LayerMask detectionLayer;

    MovementHandler movementHandler;
    CollisionHandler collisionHandler;
    DirectionHandler directionHandler;


    void Start()
    {
        currentDetectionOffset = detectionOffset;
        movementHandler = GetComponent<MovementHandler>();
        collisionHandler = GetComponent<CollisionHandler>();
        directionHandler = GetComponent<DirectionHandler>();
    }

    void Update()
    {
        UpdateEdgeDetectorPos();

        bool edgeDetected = CheckEdgeDetection();
        if (((edgeDetected && collisionHandler.isGrounded) || collisionHandler.wallRight || collisionHandler.wallLeft) && !movementHandler.isTurningAround)
            StartCoroutine(movementHandler.TurnAround());

    }

    public bool CheckEdgeDetection()
    {
        var collision = Physics2D.OverlapBox((Vector2)this.transform.position + detectionOffset, detectionSize, detectionLayer);

        if (collision == null)
            return true;

        return false;
    }

    void UpdateEdgeDetectorPos()
    {
        if (directionHandler.facingRight)
            currentDetectionOffset.x = detectionOffset.x;
        else
            currentDetectionOffset.x = -detectionOffset.x;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + currentDetectionOffset, detectionSize);
    }
}
