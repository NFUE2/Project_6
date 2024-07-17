using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Object/EnemyDataSO")]
public class EnemyDataSO : CharacterDataSO
{
    public bool isRight;
    public float searchDistance;
    public float attackDistance;
    public GameObject projectile;
}
