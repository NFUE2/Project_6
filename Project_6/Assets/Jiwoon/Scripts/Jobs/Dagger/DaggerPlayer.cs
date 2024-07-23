using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Collections;

public class DaggerPlayer : MeleePlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private DashSkill dashSkill;

    [Header("Skill E")]
    [SerializeField] private StackSkill stackSkill;

    private void Start()
    {
        dashSkill.SetCooldownText(qCooldownText);
        stackSkill.SetCooldownText(eCooldownText);
    }

    public override void Attack()
    {
        if (isAttacking) return;  // ���� ���� �ƴ� ��쿡�� ����
        isAttacking = true;
        animator.SetTrigger("IsAttack");
        StartCoroutine(PerformAttackWithStackCheck());
    }

    public override void UseSkillQ()
    {
        dashSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        stackSkill.UseSkill();
    }

    private IEnumerator PerformAttackWithStackCheck()
    {
        //������ �־����� �������� �������� ������ ������.
        yield return new WaitForSeconds(0.1f); // ���� �ִϸ��̼��� Ÿ�ֿ̹� ���� ���

        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            var damageable = enemy.GetComponent<IDamagable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
                stackSkill.IncreaseStack(); // ���� �������� �Ծ��� ���� ���� ����
            }
        }

        yield return new WaitForSeconds(attackCooldown - 0.1f); // ���� ��Ÿ�� ���
        isAttacking = false;
    }
}
