using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using TMPro;

public class SwordPlayer : MeleePlayerBase
{
    public PlayerData PlayerData;
    public TextMeshProUGUI qCooldownText; // Q 스킬 쿨타임을 표시하는 UI 텍스트 요소
    public TextMeshProUGUI eCooldownText; // E 스킬 쿨타임을 표시하는 UI 텍스트 요소

    [Header("Animation Data")]
    public Animator animator; // 향후 애니메이션 에셋 추가 => Sword를 위한 애니메이션 컨트롤러

    [Header("Skill Q")]
    [SerializeField] private GuardSkill guardSkill; // GuardSkill 인스턴스

    [Header("Skill E")]
    [SerializeField] private ProjectileSkill projectileSkill; // ProjectileSkill 인스턴스

    private void Start()
    {
        guardSkill.SetCooldownText(qCooldownText);
        projectileSkill.SetCooldownText(eCooldownText);
    }

    public override void UseSkillQ()
    {
        guardSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        projectileSkill.UseSkill();
    }
}
