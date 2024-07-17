using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using TMPro;

public class SwordPlayer : MeleePlayerBase
{
    //���� Ŭ������ �޾ƿ���
    [Header("Skill Q")]
    [SerializeField] private GuardSkill guardSkill; // GuardSkill �ν��Ͻ�

    [Header("Skill E")]
    [SerializeField] private ProjectileSkill projectileSkill; // ProjectileSkill �ν��Ͻ�


    //����Ŭ�������� ó��
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
