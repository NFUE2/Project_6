using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;

public class RiflePlayer : RangedPlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private TargetSkill targetSkill; // TargetSkill �ν��Ͻ�

    [Header("Skill E")]
    [SerializeField] private GrenadeSkill grenadeSkill; // GrenadeSkill �ν��Ͻ�

    private void Start()
    {
        targetSkill.SetCooldownText(qCooldownText);
        grenadeSkill.SetCooldownText(eCooldownText);
    }

    public override void UseSkillQ()
    {
        Debug.Log("UseSkillQ ȣ���"); // ����� �޽��� �߰�
        targetSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        grenadeSkill.UseSkill();
    }
}
