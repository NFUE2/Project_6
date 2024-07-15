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
        if (isAttacking) return;  //������ ������ �ƴ� ������ ���ݱ������� ������Ÿ���� �������� �����Ҽ��ְ� ���ּ���
        isAttacking = true;
        GetComponent<Animator>().SetTrigger("Attack"); //animator ������Ʈ�� �ֻ��� Ŭ�������� ����
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ��� �޼���
    // �Ʒ�����̸� ���ݹ����� �ƴ϶� �÷��̾ ���� ������ �������� �����Ͱ����ϴ�
    // GetComponent����� �������ּ���, ����ȭ�� ��������ϴ�
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
