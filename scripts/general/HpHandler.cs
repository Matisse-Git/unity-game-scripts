using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpHandler : MonoBehaviour
{

    [Header("HP Values")]
    public float HP;
    bool invincible;
    public float invincibilityLength;

    public void Damage(float damage){
        if (!invincible)
        {
            HP -= damage;

            if (HP <= 0){
                return;
            }
            
            StartCoroutine(StartDamageInvincibility());
        }
    }

    IEnumerator StartDamageInvincibility()
    {
        invincible = true;

        yield return new WaitForSeconds(invincibilityLength);

        invincible = false;
    }
}
