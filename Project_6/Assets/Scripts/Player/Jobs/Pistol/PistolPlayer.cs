using Photon.Pun;
using UnityEngine;
using TMPro;

public class PistolPlayer : RangedPlayerBase
{
    protected bool isAttackCooldown = false;
    protected int attackCount = 0;
    protected float cooldownDuration = 0.5f;

    [Header("Skill Q")]
    [SerializeField] private FanningSkill fanningSkill;

    [Header("Skill E")]
    [SerializeField] private RollingSkill rollingSkill;

    private void Start()
    {
        fanningSkill.SetCooldownText(qCooldownText);
        rollingSkill.SetCooldownText(eCooldownText);
    }

    public override void Attack()
    {
        if (!isAttackCooldown)
        {
            attackCount++;
            if (attackCount >= 6)
            {
                isAttackCooldown = true;
                attackCount = 0;
                lastAttackTime = Time.time;
            }
        }
    }

    private void Update()
    {
        UpdateCooldown();
    }

    private void UpdateCooldown()
    {
        if (isAttackCooldown && Time.time - lastAttackTime >= cooldownDuration)
        {
            isAttackCooldown = false;
        }
    }

    public override void UseSkillQ()
    {
        fanningSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        rollingSkill.UseSkill();
    }
}

