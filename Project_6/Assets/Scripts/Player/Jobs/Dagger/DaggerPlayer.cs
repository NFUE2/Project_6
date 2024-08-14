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
        // 기본 공격 로직 실행 (상속된 Attack 메서드 사용)
        base.Attack();

        // 스택 증가 로직 추가
        StartCoroutine(StackIncreaseCheck());
    }

    private IEnumerator StackIncreaseCheck()
    {
        // 공격이 끝날 때까지 기다림
        yield return new WaitForSeconds(playerData.attackTime);

        // 공격 범위 내 적을 다시 확인하고 스택 증가 처리
        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        bool hasIncreasedStack = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            var damagable = enemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                // 첫 번째로 감지된 적에 대해 스택을 증가시킴
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
