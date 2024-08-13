using Photon.Pun;
using UnityEngine;
using TMPro;

public class BowPlayer : RangedPlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private WireArrowSkill wireArrowSkill;

    [Header("Skill E")]
    [SerializeField] private BombArrowSkill bombArrowSkill;

    private void Start()
    {
        //wireArrowSkill.SetCooldownText(qCooldownText);
        //bombArrowSkill.SetCooldownText(eCooldownText);
        wireArrowSkill.SetCooldownImage(qCooldownImage);
        bombArrowSkill.SetCooldownImage(eCooldownImage);
    }

    public override void UseSkillQ()
    {
        wireArrowSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        bombArrowSkill.UseSkill();
    }
}
