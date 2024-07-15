using System.Collections;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public abstract void UseSkill();
    public abstract IEnumerator Cooldown();
}