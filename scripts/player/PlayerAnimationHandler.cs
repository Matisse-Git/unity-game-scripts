using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : AnimationHandler
{

    GravityHandler gravityHandler;
    DirectionHandler directionHandler;
    CollisionHandler collisionHandler;
    DashHandler dashHandler;
    WallJumpHandler wallJumpHandler;
    JumpHandler jumpHandler;
    MovementHandler movementHandler;

    PlayerMovementHandler playerMovementHandler;

    PlayerCombatHandler combatHandler;

    SpriteRenderer spriteRenderer;
    InputMaster input;

    bool flipSprite;
    bool isWalking;


    void OnEnable() => input.player.Enable();
    void OnDisable() => input.player.Disable();
    private void Awake()
    {
        input = new InputMaster();
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
        playerMovementHandler = GetComponent<PlayerMovementHandler>();

        combatHandler = GetComponent<PlayerCombatHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckFlipSprite();
        CheckGrounded();
        CheckJumping();
        CheckIdle();
        CheckWalking();
        CheckDashing();
        CheckAttacking();
        CheckWallClinging();
    }


    void CheckFlipSprite()
    {
        if (directionHandler.facingRight)
            flipSprite = false;
        else
            flipSprite = true;

        if (directionHandler.canChangeDirection)
            spriteRenderer.flipX = flipSprite;
    }

    void CheckIdle()
    {
        if (playerMovementHandler.CheckIdle())
        {
            var previousIdleTime = animator.GetFloat("idleTime");
            var newIdleTime = previousIdleTime += Time.deltaTime;
            animator.SetFloat("idleTime", newIdleTime);
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetFloat("idleTime", 0);
            animator.SetBool("isIdle", false);
        }
    }

    void CheckWalking()
    {
        isWalking = input.player.movement.ReadValue<float>() != 0;
        animator.SetBool("isWalking", isWalking);
    }

    void CheckGrounded()
    {
        animator.SetBool("isGrounded", collisionHandler.isGrounded);
    }

    void CheckJumping()
    {
        animator.SetBool("isJumping", gravityHandler.isJumping);
        animator.SetBool("isFalling", gravityHandler.isFalling);
    }

    void CheckWallClinging()
    {
        animator.SetBool("isWallClinging", wallJumpHandler.isWallClinging);
    }

    void CheckDashing()
    {
        animator.SetBool("isDashing", dashHandler.isDashing);
    }

    void CheckAttacking()
    {
        animator.SetBool("isAttacking", combatHandler.isAttacking);
    }

}
