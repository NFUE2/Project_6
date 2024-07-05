using UnityEngine;


[CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Scriptable Object/CharacterDataSO")]
public class CharacterDataSO : ObjectSO
{
    public float maxHP;
    public float moveSpeed;
    public float attackTime;
    public float attackDamage;
}
