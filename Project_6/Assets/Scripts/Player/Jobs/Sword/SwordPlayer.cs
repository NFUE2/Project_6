using Photon.Pun;
using UnityEngine;
using TMPro;

public class SwordPlayer : MeleePlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private GuardSkill guardSkill;

    [Header("Skill E")]
    [SerializeField] private ProjectileSkill projectileSkill;

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
