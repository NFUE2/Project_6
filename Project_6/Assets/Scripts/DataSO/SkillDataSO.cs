
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Object/SkillDataSO")]

public class SkillDataSO : ObjectSO
{
    public Sprite image;

    public float range;
    public float coolTime;
    public float duration;
    public float damage;
    public float buff;
}
