using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;

public class RiflePlayer : RangedPlayerBase
{
    //해당 클래스의 상위 클래스를 받아와주세요
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
        targetSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        grenadeSkill.UseSkill();
    }
}
