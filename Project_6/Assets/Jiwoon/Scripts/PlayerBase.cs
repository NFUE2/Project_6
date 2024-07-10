using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    protected float qSkillCooldown;
    protected float lastQActionTime;
    protected float eSkillCooldown;
    protected float lastEActionTime;
    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();

}
