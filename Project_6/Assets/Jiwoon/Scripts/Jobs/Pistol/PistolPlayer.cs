using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class PistolPlayer : RangedPlayerBase
{
    public PlayerData PlayerData; //여기서 받지말고 최상위에서 받아주세요

    //이 부분도 최상위에서 처리
    public TextMeshProUGUI qCooldownText; // Q 스킬 쿨타임을 표시하는 UI 텍스트 요소
    public TextMeshProUGUI eCooldownText; // E 스킬 쿨타임을 표시하는 UI 텍스트 요소
    //=================================

    //스킬의 부모클래스로 받아주세요
    [Header("Skill Q")]
    [SerializeField] private FanningSkill fanningSkill; // FanningSkill 인스턴스

    [Header("Skill E")]
    [SerializeField] private RollingSkill rollingSkill; // RollingSkill 인스턴스
    //=================================


    private void Start()
    {
        mainCamera = Camera.main; //불필요해 보입니다, 이미 RangedPlayerBase에서 처리중

        //최상위에서 처리
        fanningSkill.SetCooldownText(qCooldownText);
        rollingSkill.SetCooldownText(eCooldownText);
        //======================
    }

    public override void Attack()
    {
        // 6번 공격 후 장전
        if (attackCount >= 6)
        {
            isAttackCooldown = true;
            attackCount = 0; //0으로 정해지는건있는데 올라가는건 없네요
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
