using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Object/SkillDataSO")]

public class SkillDataSO : ObjectSO
{
    public float coolTime;

    public float lastActionTime;

    public bool isQuickAction;

    public Sprite image;
}
