using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    public PlayerData playerData;
    protected float qSkillCooldown;
    protected float lastQActionTime;
    protected float eSkillCooldown;
    protected float lastEActionTime;

    protected float qSkillCooldownRemaining;
    protected float eSkillCooldownRemaining;
    protected virtual void Update()
    {
        if (qSkillCooldownRemaining > 0)
        {
            qSkillCooldownRemaining -= Time.deltaTime;
            if (qSkillCooldownRemaining < 0)
                qSkillCooldownRemaining = 0;
        }

        if (eSkillCooldownRemaining > 0)
        {
            eSkillCooldownRemaining -= Time.deltaTime;
            if (eSkillCooldownRemaining < 0)
                eSkillCooldownRemaining = 0;
        }
    }

    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();

}
