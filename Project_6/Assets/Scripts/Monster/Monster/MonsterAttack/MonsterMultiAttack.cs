using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMultiAttack : MonsterAttack
{
    //public MonsterMultiAttack(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Attack()
    {
        Vector2 myPos = transform.position;
        int direction = controller.isRight ? 1 : -1;
        //LayerMask target = controller.targetLayer;
        float attackDistance = data.attackDistance; //공격거리 필요

        RaycastHit2D[] ray = Physics2D.RaycastAll(myPos, Vector2.right * direction * attackDistance, 5.0f, target);

        foreach(RaycastHit2D hit in ray)
        {
            if (hit.transform.TryGetComponent(out IDamagable player))
                player.TakeDamage(data.attackDamage);
        }
    }
}
