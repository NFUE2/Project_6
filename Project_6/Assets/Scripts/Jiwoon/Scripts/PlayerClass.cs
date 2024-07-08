using UnityEngine;

[CreateAssetMenu(fileName = "PlayerClass", menuName = "PlayerClass")]
public class PlayerClass : ScriptableObject
{
    public string className;
    public IAttack attack;
    public ISkillQ skillQ;
    public ISkillE skillE;
}
