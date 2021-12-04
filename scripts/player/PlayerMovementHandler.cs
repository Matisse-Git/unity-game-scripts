using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionHandler), typeof(MovementHandler), typeof(DirectionHandler))]
[RequireComponent(typeof(DashHandler), typeof(GravityHandler), typeof(WallJumpHandler))]
[RequireComponent(typeof(JumpHandler))]
public class PlayerMovementHandler : Rigidbody2DHandler
{
    InputMaster input;

    GravityHandler gravityHandler;
    DirectionHandler directionHandler;
    CollisionHandler collisionHandler;
    DashHandler dashHandler;
    WallJumpHandler wallJumpHandler;
    JumpHandler jumpHandler;
    MovementHandler movementHandler;

    public bool isIdle;


    void OnEnable() => input.player.Enable();
    void OnDisable() => input.player.Disable();

    void Awake()
    {
        input = new InputMaster();
        input.player.jump.performed += _ => jumpHandler.Jump();
        input.player.jump.performed += _ => wallJumpHandler.WallJump();
        input.player.jump.canceled += _ => jumpHandler.CancelJump();
        input.player.dash.performed += _ => dashHandler.Dash();
    }

    protected override void Start()
    {
        base.Start();
        gravityHandler = GetComponent<GravityHandler>();
        directionHandler = GetComponent<DirectionHandler>();
        collisionHandler = GetComponent<CollisionHandler>();
        dashHandler = GetComponent<DashHandler>();
        wallJumpHandler = GetComponent<WallJumpHandler>();
        jumpHandler = GetComponent<JumpHandler>();
        movementHandler = GetComponent<MovementHandler>();
    }

    void Update()
    {
        movementHandler.SetAxisX(input.player.movement.ReadValue<float>());
        isIdle = CheckIdle();
    }


    public bool CheckIdle()
    {
        if (movementHandler.axisX == 0 && collisionHandler.isGrounded && !gravityHandler.isJumping && !gravityHandler.isFalling && !dashHandler.isDashing)
        {
            return true;
        }
        else
            return false;
    }
}