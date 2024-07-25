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
        if (isAttacking) return; // ���� ���� �ƴ� ��쿡�� ����
        isAttacking = true;
        animator.SetTrigger("IsAttack");
        StartCoroutine(PerformAttackWithStackCheck());
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator PerformAttackWithStackCheck()
    {
        yield return new WaitForSeconds(0.1f); // ���� �ִϸ��̼��� Ÿ�ֿ̹� ���� ���

        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        bool hasIncreasedStack = false; // ������ �̹� �����ߴ��� Ȯ���ϴ� �÷���

        foreach (Collider2D enemy in hitEnemies)
        {
            var damagable = enemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(attackDamage);

                if (!hasIncreasedStack)
                {
                    stackSkill.IncreaseStack(); // ���� �������� �Ծ��� ���� ���� ����
                    hasIncreasedStack = true; // ������ �� �� ���������� ǥ��
                }
            }
        }
    }

    public override void UseSkillQ()
    {
        dashSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        stackSkill.UseSkill();
    }
}
