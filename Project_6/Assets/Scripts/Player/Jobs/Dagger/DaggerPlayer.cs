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
        if (isAttacking) return; // 공격 중이거나 쿨다운 중인 경우 공격 불가
        isAttacking = true;
        animator.SetTrigger("IsAttack");
        StartCoroutine(AttackCooldown());
    }

    // 애니메이션 이벤트에서 호출될 메서드
    public new void PerformAttack()
    {
        StartCoroutine(PerformAttackWithStackCheck());
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
                ApplyKnockback(enemy); // 넉백 적용

                if (!hasIncreasedStack)
                {
                    stackSkill.IncreaseStack(); // 적이 데미지를 입었을 때만 스택 증가
                    hasIncreasedStack = true; // 스택이 한 번 증가했음을 표시
                    Debug.Log("스택 증가: " + stackSkill.currentStack);
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
