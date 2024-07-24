using System.Collections;
using UnityEngine;

public abstract class MeleePlayerBase : PlayerBase
{
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    public float attackCooldown = 1f;  // ���� ��Ÿ�� ����
    protected bool isAttacking = false;
    public Vector2 attackSize; // ���� �ڽ��� ũ��
    public Vector2 attackOffset; // ���� �ڽ��� ������

    protected void Awake() // �ֻ��� Ŭ�������� ȣ��ǵ��� ����
    {
        animator = GetComponent<Animator>();
    }

    public override void Attack()
    {
        if (isAttacking) return;  // ���� ���� �ƴ� ��쿡�� ����
        isAttacking = true;
        animator.SetTrigger("IsAttack");
        StartCoroutine(AttackCooldown());
    }

    protected IEnumerator AttackCooldown() // ���� ��Ÿ���� ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ��� �޼���
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
            return basePosition + new Vector2(-attackOffset.x, attackOffset.y); // ������ �ٶ� ��
        }
        else
        {
            return basePosition + attackOffset; // �������� �ٶ� ��
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
