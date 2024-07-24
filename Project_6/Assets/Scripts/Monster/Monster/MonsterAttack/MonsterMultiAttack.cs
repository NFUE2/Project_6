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

        //base.Attack();

        //Vector2 myPos = ((Vector2)transform.position + controller.offsetPos) * Direction();
        //int direction = TargetisRight() ? 1 : -1;
        //LayerMask target = controller.targetLayer;
        //float attackDistance = data.attackDistance; //공격거리 필요
        //RaycastHit2D[] ray = Physics2D.RaycastAll(myPos, Vector2.right * Direction() * attackDistance, 5.0f, target);
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitBox.position,hitBox.localScale,0.0f,target);

        foreach(Collider2D hit in cols)
        {
            if (hit.transform.TryGetComponent(out IDamagable player))
                player.TakeDamage(data.attackDamage);
        }
    }
}
