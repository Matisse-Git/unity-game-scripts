using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatHandler : MonoBehaviour
{
    InputMaster input;

    MeleeCombatHandler combatHandler;

    [Header("Attack Values")]
    Vector2 attackPos;
    public Vector2 attackArea;
    public Vector2 attackOffset;
    public float attackDamage;
    public float attackLength;
    public float attackDelay;

    [Header("Attack Status")]
    public bool isAttacking;
    public bool canAttack;
    public bool delayingAttack;

    [Header("Enemy Values")]
    public LayerMask enemyLayer;

    GravityHandler gravityHandler;
    DirectionHandler directionHandler;
    CollisionHandler collisionHandler;
    DashHandler dashHandler;
    WallJumpHandler wallJumpHandler;
    JumpHandler jumpHandler;
    MovementHandler movementHandler;


    void OnEnable() => input.player.Enable();
    void OnDisable() => input.player.Disable();

    void Awake()
    {
        input = new InputMaster();
        input.player.attack.performed += _ => StartAttack();

        combatHandler = GetComponent<MeleeCombatHandler>();

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
        UpdateAttackPos();
        CheckCanAttack();
    }

    void UpdateAttackPos()
    {
        var facingRight = directionHandler.facingRight;

        attackPos = transform.position;

        if ((facingRight && attackOffset.x < 0) || (!facingRight && attackOffset.x > 0))
            attackOffset.x *= -1;
    }

    void StartAttack()
    {
        if (!delayingAttack)
        {
            if (canAttack)
                StartCoroutine(Attack());
        }
        else
            StartCoroutine(AttackWhenReady());
    }

    IEnumerator Attack()
    {
        directionHandler.canChangeDirection = false;
        StartCoroutine(AttackLength());
        
        List<Collider2D> enemiesHit = new List<Collider2D>();
        while (isAttacking)
        {
            var enemiesFound = combatHandler.GetEnemiesInArea(attackPos, attackArea, attackOffset, enemyLayer);

            foreach (var foundEnemy in enemiesFound)
            {
                if (!enemiesHit.Contains(foundEnemy))
                {
                    foundEnemy.GetComponentInParent<HpHandler>().Damage(attackDamage);
                    enemiesHit.Add(foundEnemy);
                }
            }

            yield return null;
        }
    }

    IEnumerator AttackLength()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackLength);

        isAttacking = false;
        directionHandler.canChangeDirection = true;

        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        delayingAttack = true;

        yield return new WaitForSeconds(attackDelay);

        delayingAttack = false;
    }

    IEnumerator AttackWhenReady()
    {
        while (delayingAttack)
        {
            yield return null;
        }

        StartCoroutine(Attack());
    }

    void CheckCanAttack()
    {
        canAttack = !dashHandler.isDashing && !isAttacking;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos + attackOffset, attackArea);
    }
}
