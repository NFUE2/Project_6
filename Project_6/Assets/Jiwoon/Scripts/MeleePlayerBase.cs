using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleePlayerBase : PlayerBase
{
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    protected bool isAttacking = false;

    public override void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        GetComponent<Animator>().SetTrigger("Attack");
    }

    // 애니메이션 이벤트에서 호출될 메서드
    public void EnableAttackCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void DisableAttackCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            other.GetComponent<IDamagable>().TakeDamage(attackDamage);
        }
    }
}
