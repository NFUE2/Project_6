using System.Collections;
using TMPro;
using UnityEngine;

public class GuardSkill : MonoBehaviour
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 1.0f; // 가드 지속 시간
    public PlayerData PlayerData;
    private float lastActionTime;
    private TextMeshProUGUI cooldownText;

    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }

    public void UseSkill()
    {
        if (IsGuard)
        {
            ExitGuard();
        }
        else
        {
            if (Time.time - lastActionTime < PlayerData.SkillQCooldown) return; // Q 스킬 쿨타임 체크

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

    private void Update()
    {
        if (cooldownText != null)
        {
            if (Time.time - lastActionTime >= PlayerData.SkillQCooldown)
            {
                cooldownText.text = "Q스킬 쿨타임 완료"; // 쿨타임 완료 텍스트 갱신
            }
            else
            {
                float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastActionTime);
                cooldownText.text = $"{remainingTime:F1}"; // 쿨타임 텍스트 갱신
            }
        }
    }
}