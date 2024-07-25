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
        if (isAttacking) return; // 공격 중이 아닌 경우에만 공격
        isAttacking = true;
        animator.SetTrigger("IsAttack");
        StartCoroutine(PerformAttackWithStackCheck());
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator PerformAttackWithStackCheck()
    {
        yield return new WaitForSeconds(0.1f); // 공격 애니메이션의 타이밍에 맞춰 대기

        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        bool hasIncreasedStack = false; // 스택이 이미 증가했는지 확인하는 플래그

        foreach (Collider2D enemy in hitEnemies)
        {
            var damagable = enemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(attackDamage);

                if (!hasIncreasedStack)
                {
                    stackSkill.IncreaseStack(); // 적이 데미지를 입었을 때만 스택 증가
                    hasIncreasedStack = true; // 스택이 한 번 증가했음을 표시
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
