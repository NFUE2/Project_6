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
        if (isAttacking) return;  // 공격 중이 아닌 경우에만 공격
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
        //공격이 넣어지고 데미지가 들어갔을때만 스택이 오른다.
        yield return new WaitForSeconds(0.1f); // 공격 애니메이션의 타이밍에 맞춰 대기

        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            var damageable = enemy.GetComponent<IDamagable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
                stackSkill.IncreaseStack(); // 적이 데미지를 입었을 때만 스택 증가
            }
        }

        yield return new WaitForSeconds(attackCooldown - 0.1f); // 남은 쿨타임 대기
        isAttacking = false;
    }
}
