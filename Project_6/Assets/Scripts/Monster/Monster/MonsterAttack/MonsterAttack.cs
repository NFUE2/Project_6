using Unity.VisualScripting;
using UnityEngine;

public abstract class MonsterAttack : MonoBehaviour
{
    //protected MonsterStateMachine stateMachine;
    protected MonsterController controller;
    protected EnemyDataSO data;
    //MonsterAttackState state;
    //protected float attackTime;
    //protected float damage;
    public LayerMask target;
    public AudioClip attackClip;
    public abstract void Attack();

    //protected MonsterAttack(MonsterStateMachine stateMachine)
    //{
    //    this.stateMachine = stateMachine;
    //    attackTime = stateMachine.controller.data.attackTime;
    //}

    private void Awake()
    {
        controller = GetComponentInParent<MonsterController>();
        data = controller.data;

        //attackTime = controller.data.attackTime;
        //damage = controller.data.attackDamage;
    }

    //public void Initailize(MonsterAttackState state)
    //{
    //    this.state = state;
    //}

        
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
    //    //Debug.Log("���� ����");
    //    state.isAttacking = false;
    //}
}
