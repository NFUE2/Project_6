using UnityEngine;

public class MonsterStateMachine : StateMachine
{
    public MonsterController controller;

    public MonsterIdleState idleState;
    public MonsterTrackState trackState;
    public MonsterAttackState attackState;
    public MonsterDieState dieState;

    public MonsterStateMachine(MonsterController controller)
    {
        this.controller = controller;

        idleState = new MonsterIdleState(this);
        trackState = new MonsterTrackState(this);
        attackState = new MonsterAttackState(this);
        dieState = new MonsterDieState(this);

        controller.condition.OnSpawn += Spawn;
        controller.condition.OnDie += Die;

        controller.condition.enabled = true;
    }

    private void Spawn()
    {
        ChangeState(idleState);
    }

    private void Die()
    {
        ChangeState(dieState);
    }
}
