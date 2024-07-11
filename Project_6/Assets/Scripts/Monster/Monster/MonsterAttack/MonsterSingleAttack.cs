using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSingleAttack : MonsterAttack
{
    public MonsterSingleAttack(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Attack()
    {
        Vector2 myPos = stateMachine.controller.transform.position;
        int direction = stateMachine.controller.transform.localScale.x == 1 ? 1 : -1;
        LayerMask target = stateMachine.controller.targetLayer;
        //int attackRange = stateMachine.controller.data.att
        RaycastHit2D ray = Physics2D.Raycast(myPos, Vector2.right * direction, 5.0f, target);

        if (ray.transform.TryGetComponent(out IDamagable player))
            player.TakeDamage(stateMachine.controller.data.attackDamage);
    }
}
