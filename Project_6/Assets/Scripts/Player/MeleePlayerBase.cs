using System.Collections;
using UnityEngine;

public abstract class MeleePlayerBase : PlayerBase
{
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    public float attackCooldown = 1f;  // 공격 쿨타임 설정
    protected bool isAttacking = false;
    public Vector2 attackSize; // 공격 박스의 크기
    public Vector2 attackOffset; // 공격 박스의 오프셋

    protected void Awake() // 최상위 클래스에서 호출되도록 설정
    {
        animator = GetComponent<Animator>();
    }

    public override void Attack()
    {
        if (isAttacking) return;  // 공격 중이 아닌 경우에만 공격
        isAttacking = true;
        animator.SetTrigger("IsAttack");
        StartCoroutine(AttackCooldown());
    }

    protected IEnumerator AttackCooldown() // 공격 쿨타임을 위한 코루틴
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // 애니메이션 이벤트에서 호출될 메서드
    public void PerformAttack()
    {
        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<IDamagable>()?.TakeDamage(attackDamage);
        }
    }

    protected Vector2 CalculateAttackPosition()
    {
        Vector2 basePosition = (Vector2)transform.position;

        if (transform.localScale.x < 0)
        {
            return basePosition + new Vector2(-attackOffset.x, attackOffset.y); // 왼쪽을 바라볼 때
        }
        else
        {
            return basePosition + attackOffset; // 오른쪽을 바라볼 때
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
