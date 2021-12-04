using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public Transform playerTransform;

    [Header("Follow Values")]
    public float cameraDistance;
    public Vector2 cameraOffset;
    public float smoothFollowStep;
    Vector3 posFrom;
    Vector3 posTo;

    [Header("Collider Values")]
    public LayerMask colliderLayer;
    public Vector2 colliderSize;
    Vector2 leftSide;
    Vector2 rightSide;
    Vector2 topSide;
    Vector2 bottomSide;
    public float detectionRadius;

    [Header("Collider Statuses")]
    public bool colliderLeft;
    public bool colliderRight;
    public bool colliderTop;
    public bool colliderBottom;

    void Update()
    {
        UpdateSides();
        CheckColliders();
        UpdateMoveTowards();
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    }

    void UpdateMoveTowards()
    {
        posFrom = new Vector3(transform.position.x, transform.position.y, cameraDistance);
        posTo = posFrom;

        if ((!colliderLeft || (colliderLeft && playerTransform.position.x >= posFrom.x)) && !colliderRight)
        {
            posTo.x = playerTransform.position.x + cameraOffset.x;
        }
        if ((!colliderRight || (colliderRight && playerTransform.position.x <= posFrom.x)) && !colliderLeft)
        {
            posTo.x = playerTransform.position.x + cameraOffset.x;
        }
        if ((!colliderTop || (colliderTop && playerTransform.position.y <= posFrom.y)) && !colliderBottom)
        {
            posTo.y = playerTransform.position.y + cameraOffset.y;
        }
        if ((!colliderBottom || (colliderBottom && playerTransform.position.y >= posFrom.y)) && !colliderTop)
        {
            posTo.y = playerTransform.position.y + cameraOffset.y;
        }
    }

    void MoveToPlayer()
    {
        var moveTowards = Vector3.Lerp(posFrom, posTo, smoothFollowStep);
        transform.position = moveTowards;
    }

    void UpdateSides()
    {
        leftSide = new Vector2(transform.position.x - (colliderSize.x / 2), transform.position.y);
        rightSide = new Vector2(transform.position.x + (colliderSize.x / 2), transform.position.y);
        topSide = new Vector2(transform.position.x, transform.position.y + (colliderSize.y / 2));
        bottomSide = new Vector2(transform.position.x, transform.position.y - (colliderSize.y / 2));
    }

    void CheckColliders()
    {
        var leftCheck = Physics2D.OverlapCircle(leftSide, detectionRadius, colliderLayer);
        if (leftCheck != null)
            colliderLeft = true;
        else
            colliderLeft = false;

        var rightCheck = Physics2D.OverlapCircle(rightSide, detectionRadius, colliderLayer);
        if (rightCheck != null)
            colliderRight = true;
        else
            colliderRight = false;

        var topCheck = Physics2D.OverlapCircle(topSide, detectionRadius, colliderLayer);
        if (topCheck != null)
            colliderTop = true;
        else
            colliderTop = false;

        var bottomCheck = Physics2D.OverlapCircle(bottomSide, detectionRadius, colliderLayer);
        if (bottomCheck != null)
            colliderBottom = true;
        else
            colliderBottom = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, colliderSize);
        Gizmos.DrawWireSphere(leftSide, detectionRadius);
        Gizmos.DrawWireSphere(rightSide, detectionRadius);
        Gizmos.DrawWireSphere(topSide, detectionRadius);
        Gizmos.DrawWireSphere(bottomSide, detectionRadius);

        Gizmos.color = Color.cyan;
        if (colliderLeft)
            Gizmos.DrawWireSphere(leftSide, detectionRadius);
        if (colliderRight)
            Gizmos.DrawWireSphere(rightSide, detectionRadius);
        if (colliderTop)
            Gizmos.DrawWireSphere(topSide, detectionRadius);
        if (colliderBottom)
            Gizmos.DrawWireSphere(bottomSide, detectionRadius);
    }
}
