using Photon.Pun;
using UnityEngine;
using TMPro;

public class HammerPlayer : MeleePlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private ShieldSkill shieldSkill;

    [Header("Skill E")]
    [SerializeField] private ChargeSkill chargeSkill;

    private void Start()
    {
        //shieldSkill.SetCooldownText(qCooldownText);
        //chargeSkill.SetCooldownText(eCooldownText);
        shieldSkill.SetCooldownImage(qCooldownImage);
        chargeSkill.SetCooldownImage(eCooldownImage);
    }

    public override void UseSkillQ()
    {
        shieldSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        chargeSkill.UseSkill();
    }
}
