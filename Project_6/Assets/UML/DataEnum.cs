using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEnum
{
    public enum StateType
    {
        MaxHP,
        MoveSpeed,
        AttackTime,
        AttackDamage,
        Skill_Q_CoolTimeDecrease,
        Skill_E_CoolTimeDecrease,
    }

    public enum ApplyType
    {
        Percent,
        Amount,
    }

    public enum UsedType
    {
        Heal,
        Enhance,
    }
}
