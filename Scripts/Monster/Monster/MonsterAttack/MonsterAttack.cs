using UnityEngine;

public abstract class MonsterAttack : MonoBehaviour
{
    protected MonsterController controller;
    protected EnemyDataSO data;
    public LayerMask target;
    public AudioClip attackClip;
    public abstract void Attack();

    private void Awake()
    {
        controller = GetComponentInParent<MonsterController>();
        data = controller.data;
    }

    public int Direction()
    {
        Vector2 dir = controller.target.position - transform.position;
        return dir.x > 0 ? 1 : -1;
    }

    public Vector2 Direction(Vector2 fire, Vector2 target)
    {
        return (target - fire).normalized;
    }

    protected void AttackClip()
    {
        SoundManager.instance.Shot(attackClip);

    }
    //public void ExitAttack()
    //{
    //    //Debug.Log("공격 종료");
    //    state.isAttacking = false;
    //}
}
