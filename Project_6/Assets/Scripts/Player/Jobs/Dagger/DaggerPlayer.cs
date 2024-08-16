using Photon.Pun;
using UnityEngine;

public class DaggerPlayer : MeleePlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private DashSkill dashSkill;

    [Header("Skill E")]
    [SerializeField] private StackSkill stackSkill;

    private void Start()
    {
        dashSkill.SetCooldownImage(qCooldownImage);
        stackSkill.SetCooldownImage(eCooldownImage);
    }

    public override void Attack()
    {
        base.Attack();
        StartCoroutine(stackSkill.StackIncreaseCheck(playerData.attackTime));
    }

    public override void UseSkillQ()
    {
        dashSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        stackSkill.UseSkill();
    }
}
