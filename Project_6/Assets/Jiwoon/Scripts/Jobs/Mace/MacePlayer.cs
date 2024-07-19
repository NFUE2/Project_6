using Photon.Pun;
using UnityEngine;
using TMPro;

public class MacePlayer : MeleePlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private HealAndBoostSkill healAndBoostSkill;

    [Header("Skill E")]
    [SerializeField] private MaceChargeSkill maceChargeSkill;

    private void Start()
    {
        healAndBoostSkill.SetCooldownText(qCooldownText);
        maceChargeSkill.SetCooldownText(eCooldownText);
    }

    public override void UseSkillQ()
    {
        healAndBoostSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        maceChargeSkill.UseSkill();
    }

    // Attack 메서드는 MeleePlayerBase에서 상속받음
}
