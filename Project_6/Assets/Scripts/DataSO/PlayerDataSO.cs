
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Object/PlayerDataSO")]

public class PlayerDataSO : CharacterDataSO
{
    public float jumpForce;
    public float SkillQCooldown;
    public float SkillECooldown;
    public float attackCooldown;
    public float walkSpeed;
    public float runSpeed;
    public float playerdefense;

}
