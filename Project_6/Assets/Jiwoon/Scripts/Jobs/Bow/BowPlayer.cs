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
        wireArrowSkill.SetCooldownText(qCooldownText);
        bombArrowSkill.SetCooldownText(eCooldownText);
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
