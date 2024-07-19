using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMultiAttack : MonsterAttack
{
    //public MonsterMultiAttack(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Attack()
    {
        Vector2 myPos = (Vector2)transform.position + controller.offsetPos;
        int direction = controller.isRight ? 1 : -1;
        //LayerMask target = controller.targetLayer;
        float attackDistance = data.attackDistance; //공격거리 필요
        Debug.Log(direction);
        RaycastHit2D[] ray = Physics2D.RaycastAll(myPos, Vector2.right * direction * attackDistance, 5.0f, target);

        Debug.Log(ray.Length);

        foreach(RaycastHit2D hit in ray)
        {
            if (hit.transform.TryGetComponent(out IDamagable player))
                player.TakeDamage(data.attackDamage);
        }
    }
}
