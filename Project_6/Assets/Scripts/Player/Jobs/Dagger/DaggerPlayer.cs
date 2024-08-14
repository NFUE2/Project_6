using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DaggerPlayer : MeleePlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private DashSkill dashSkill;

    [Header("Skill E")]
    [SerializeField] private StackSkill stackSkill;

    private void Start()
    {
        //dashSkill.SetCooldownText(qCooldownText);
        //stackSkill.SetCooldownText(eCooldownText);
        dashSkill.SetCooldownImage(qCooldownImage);
        stackSkill.SetCooldownImage(eCooldownImage);
    }

    public override void Attack()
    {
        // �⺻ ���� ���� ���� (��ӵ� Attack �޼��� ���)
        base.Attack();

        // ���� ���� ���� �߰�
        StartCoroutine(StackIncreaseCheck());
    }

    private IEnumerator StackIncreaseCheck()
    {
        // ������ ���� ������ ��ٸ�
        yield return new WaitForSeconds(playerData.attackTime);

        // ���� ���� �� ���� �ٽ� Ȯ���ϰ� ���� ���� ó��
        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        bool hasIncreasedStack = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            var damagable = enemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                // ù ��°�� ������ ���� ���� ������ ������Ŵ
                if (!hasIncreasedStack)
                {
                    stackSkill.IncreaseStack();
                    hasIncreasedStack = true;
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
