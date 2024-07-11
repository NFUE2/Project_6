using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMultiAttack : MonsterAttack
{
    public MonsterMultiAttack(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Attack()
    {
        Vector2 myPos = stateMachine.controller.transform.position;
        int direction = stateMachine.controller.transform.localScale.x == 1 ? 1 : -1;
        LayerMask target = stateMachine.controller.targetLayer;
        //int attackRange = stateMachine.controller.data.att //공격거리 필요

        RaycastHit2D[] ray = Physics2D.RaycastAll(myPos, Vector2.right * direction, 5.0f, target);

        foreach(RaycastHit2D hit in ray)
        {
            if (hit.transform.TryGetComponent(out IDamagable player))
                player.TakeDamage(stateMachine.controller.data.attackDamage);
        }
    }
}
