using UnityEngine;
using TMPro;

public class GuardSkill : SkillBase
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 1.0f; // 가드 지속 시간
    public float damageReductionDuringGuard = 0.5f; // 가드 중 받는 데미지 감소 비율
    public PlayerDataSO PlayerData;
    public float damageReduction;


    private float originalDamageReduction;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
    }

    public override void UseSkill()
    {
        if (IsGuard)
        {
            ExitGuard();
        }
        else
        {
            if (Time.time - lastActionTime < cooldownDuration) return; // Q 스킬 쿨타임 체크

            Debug.Log("Q 스킬 사용");
            EnterGuard();
        }
    }

    private void EnterGuard()
    {
        IsGuard = true;
        originalDamageReduction = damageReduction; // 현재 데미지 감소 비율 저장
        damageReduction *= damageReductionDuringGuard; // 데미지 감소 적용
        // animator.SetBool("Guard", true);
        Invoke("ExitGuardEvent", GuardDuration); // 가드 시간 이벤트 설정
    }

    private void ExitGuard()
    {
        Debug.Log("가드 종료");
        IsGuard = false;
        damageReduction = originalDamageReduction; // 데미지 감소 비율 복원
        // animator.SetBool("Guard", false);
        lastActionTime = Time.time;
    }

    private void ExitGuardEvent()
    {
        if (IsGuard) ExitGuard();
    }
}
