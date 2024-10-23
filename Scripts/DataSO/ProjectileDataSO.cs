
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileDataSO", menuName = "Scriptable Object/ProjectileDataSO")]

public class ProjectileDataSO : ObjectSO
{

    public float moveSpeed;

    public float damage;

    public LayerMask target;

}
