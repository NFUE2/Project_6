using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleePlayerBase : PlayerBase
{
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    public float attackCooldown = 1.0f;  // ���� ��Ÿ�� ����
    protected bool isAttacking = false;

    private BoxCollider2D attackCollider;

    protected void Awake() // �ֻ��� Ŭ�������� ȣ��ǵ��� ����
    {
        animator = GetComponent<Animator>();
        attackCollider = GetComponentInChildren<BoxCollider2D>(); // ���� ������Ʈ���� ���������� �� �ݶ��̴��� �����´�.
    }

    public override void Attack()
    {
        if (isAttacking) return;  // ���� ���� �ƴ� ��쿡�� ����
        isAttacking = true;
        animator.SetTrigger("Attack");
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown() // ���� ��Ÿ���� ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ��� �޼���
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
