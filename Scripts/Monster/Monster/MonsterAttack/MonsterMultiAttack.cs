using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMultiAttack : MonsterAttack
{
    //public MonsterMultiAttack(MonsterStateMachine stateMachine) : base(stateMachine) { }
    public Transform hitBox;

    public override void Attack()
    {
        AttackClip();

        Collider2D[] cols = Physics2D.OverlapBoxAll(hitBox.position,hitBox.localScale,0.0f,target);

        foreach(Collider2D hit in cols)
        {
            if (hit.transform.TryGetComponent(out IDamagable player))
                player.TakeDamage(data.attackDamage);
        }
    }
}
