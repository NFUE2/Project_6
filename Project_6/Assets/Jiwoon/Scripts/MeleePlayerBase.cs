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
        if (isAttacking) return;  //공격중 판정이 아닌 마지막 공격기준으로 공격쿨타임이 돌았을때 공격할수있게 해주세요
        isAttacking = true;
        GetComponent<Animator>().SetTrigger("Attack"); //animator 컴포넌트를 최상위 클래스에서 설정
    }

    // 애니메이션 이벤트에서 호출될 메서드
    // 아래방식이면 공격범위가 아니라 플레이어에 닿은 적들이 데미지를 입을것같습니다
    // GetComponent방식을 수정해주세요, 최적화에 문제생깁니다
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
