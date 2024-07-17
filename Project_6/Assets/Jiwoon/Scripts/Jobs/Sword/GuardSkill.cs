using UnityEngine;
using TMPro;

public class GuardSkill : SkillBase
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 1.0f; // 가드 지속 시간
    public PlayerDataSO PlayerData;

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
            IsGuard = true;
            // animator.SetBool("Guard", true);
            Invoke("ExitGuardEvent", GuardDuration); // 가드 시간 이벤트 설정
        }
    }

    private void ExitGuard()
    {
        Debug.Log("가드 종료");
        IsGuard = false;
        // animator.SetBool("Guard", false);
        lastActionTime = Time.time;
    }

    private void ExitGuardEvent()
    {
        if (IsGuard) ExitGuard();
    }
}
