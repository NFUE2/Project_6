using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using TMPro;

public class SwordPlayer : MeleePlayerBase
{
    //상위 클래스로 받아오기
    [Header("Skill Q")]
    [SerializeField] private GuardSkill guardSkill; // GuardSkill 인스턴스

    [Header("Skill E")]
    [SerializeField] private ProjectileSkill projectileSkill; // ProjectileSkill 인스턴스


    //상위클래스에서 처리
    private void Start()
    {
        //guardSkill.SetCooldownText(qCooldownText);
        //projectileSkill.SetCooldownText(eCooldownText);
    }
    //=============================

    public override void UseSkillQ()
    {
        guardSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        projectileSkill.UseSkill();
    }
}
