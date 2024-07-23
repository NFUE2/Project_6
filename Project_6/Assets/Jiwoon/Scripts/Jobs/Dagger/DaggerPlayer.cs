using Photon.Pun;
using UnityEngine;
using TMPro;

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
        base.Attack();
        CheckAndIncreaseStack();
    }

    public override void UseSkillQ()
    {
        dashSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        stackSkill.UseSkill();
    }

    private void CheckAndIncreaseStack()
    {
        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<IDamagable>() != null)
            {
                stackSkill.IncreaseStack();
            }
        }
    }
}
