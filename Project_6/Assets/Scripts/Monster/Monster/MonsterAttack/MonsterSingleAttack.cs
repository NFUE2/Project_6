using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSingleAttack : MonsterAttack
{
    //public MonsterSingleAttack(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Attack()
    {
        //Debug.Log("АјАн");

        Vector2 myPos = transform.position;
        int direction = controller.isRight ? 1 : -1;
        //LayerMask target = controller.targetLayer;

        float attackDistance = data.attackDistance;
        RaycastHit2D ray = Physics2D.Raycast(myPos, Vector2.right * direction, attackDistance, target);
        Collider2D col = ray.collider;

        //Debug.Log(Vector2.right * direction * attackDistance);

        if (col != null && col.TryGetComponent(out IDamagable player))
        {
            player.TakeDamage(data.attackDamage);
        }
    }
}
