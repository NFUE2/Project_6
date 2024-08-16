
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Object/PlayerDataSO")]

public class PlayerDataSO : CharacterDataSO
{
    public float jumpForce;
    public float SkillQCooldown;
    public float SkillECooldown;

    //public float attackTime; //- > attackTime ����

    //public float moveSpeed; // ->MoveSpeed;
    //public float runSpeed; //MoveSpeed * 1.5f

    //public float defence; // ->defence ����
}
