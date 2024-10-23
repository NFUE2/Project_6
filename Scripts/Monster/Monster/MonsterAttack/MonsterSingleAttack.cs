using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSingleAttack : MonsterAttack
{
    //public MonsterSingleAttack(MonsterStateMachine stateMachine) : base(stateMachine) { }
    public Transform hitBox;

    public override void Attack()
    {
        AttackClip();

        Collider2D col = Physics2D.OverlapBox(hitBox.position, hitBox.localScale, 0.0f, target);

        if (col != null && col.TryGetComponent(out IDamagable player))
            player.TakeDamage(data.attackDamage);
    }
}
