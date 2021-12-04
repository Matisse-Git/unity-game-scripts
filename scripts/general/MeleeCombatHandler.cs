using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeCombatHandler : MonoBehaviour
{
    public Collider2D[] GetEnemiesInArea(Vector2 attackPos, Vector2 attackArea, Vector2 attackOffset, LayerMask enemyLayer)
    {
        return Physics2D.OverlapBoxAll(attackPos + attackOffset, attackArea, 0, enemyLayer);
    }

    public void DamageEnemiesFound(Collider2D[] enemyColliders, float attackDamage)
    {
        foreach (Collider2D col in enemyColliders)
        {
            col.GetComponentInParent<HpHandler>().Damage(attackDamage);
        }
    }
}
