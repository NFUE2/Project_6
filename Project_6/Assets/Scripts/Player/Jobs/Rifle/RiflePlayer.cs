using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;

public class RiflePlayer : RangedPlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private TargetSkill targetSkill; // TargetSkill 인스턴스

    [Header("Skill E")]
    [SerializeField] private GrenadeSkill grenadeSkill; // GrenadeSkill 인스턴스

    private void Start()
    {
        targetSkill.SetCooldownText(qCooldownText);
        grenadeSkill.SetCooldownText(eCooldownText);
    }

    public override void UseSkillQ()
    {
        Debug.Log("UseSkillQ 호출됨"); // 디버그 메시지 추가
        targetSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        grenadeSkill.UseSkill();
    }
}
