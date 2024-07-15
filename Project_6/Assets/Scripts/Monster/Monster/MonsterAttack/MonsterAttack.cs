using UnityEngine;

public abstract class MonsterAttack : MonoBehaviour
{
    protected MonsterStateMachine stateMachine;
    protected float attackTime;

    protected MonsterAttack(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        attackTime = stateMachine.controller.data.attackTime;
    }

    public abstract void Attack();
}
