using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    //스킬클래스로 이동
    protected float qSkillCooldown;
    protected float lastQActionTime;
    protected float eSkillCooldown;
    protected float lastEActionTime;

    protected float qSkillCooldownRemaining;
    protected float eSkillCooldownRemaining;
    //======================================

    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();
}
