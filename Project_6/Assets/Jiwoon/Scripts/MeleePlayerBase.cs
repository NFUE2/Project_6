using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleePlayerBase : PlayerBase
{
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    public float attackCooldown = 1.0f;  // 공격 쿨타임 설정
    protected bool isAttacking = false;

    private BoxCollider2D attackCollider;

    protected void Awake() // 최상위 클래스에서 호출되도록 설정
    {
        animator = GetComponent<Animator>();
        attackCollider = GetComponentInChildren<BoxCollider2D>(); // 하위 오브젝트에서 공격판정을 할 콜라이더를 가져온다.
    }

    public override void Attack()
    {
        if (isAttacking) return;  // 공격 중이 아닌 경우에만 공격
        isAttacking = true;
        animator.SetTrigger("Attack");
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown() // 공격 쿨타임을 위한 코루틴
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // 애니메이션 이벤트에서 호출될 메서드
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            other.GetComponent<IDamagable>().TakeDamage(attackDamage);
        }
    }
}
