using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class PistolPlayer : RangedPlayerBase
{
    public PlayerData PlayerData;
    public TextMeshProUGUI qCooldownText; // Q 스킬 쿨타임을 표시하는 UI 텍스트 요소
    public TextMeshProUGUI eCooldownText; // E 스킬 쿨타임을 표시하는 UI 텍스트 요소

    [Header("Skill Q")]
    [SerializeField] private FanningSkill fanningSkill; // FanningSkill 인스턴스

    [Header("Skill E")]
    [SerializeField] private RollingSkill rollingSkill; // RollingSkill 인스턴스

    private void Start()
    {
        mainCamera = Camera.main;
        fanningSkill.SetCooldownText(qCooldownText);
        rollingSkill.SetCooldownText(eCooldownText);
    }

    public override void Attack()
    {
        // 6번 공격 후 장전
        if (attackCount >= 6)
        {
            isAttackCooldown = true;
            attackCount = 0;
            lastAttackTime = Time.time;
        }
    }

    private void Update()
    {
        UpdateCooldown();
    }

    public override void UseSkillQ()
    {
        fanningSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        rollingSkill.UseSkill();
    }
}
