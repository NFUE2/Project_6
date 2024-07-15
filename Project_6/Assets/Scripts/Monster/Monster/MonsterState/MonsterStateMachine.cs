using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine
{
    public MonsterController controller;

    public MonsterIdleState idleState;
    public MonsterTrackState trackState;
    public MonsterAttackState attackState;

    public MonsterStateMachine(MonsterController controller)
    {
        this.controller = controller;

        idleState = new MonsterIdleState(this);
        trackState = new MonsterTrackState(this);
        attackState = new MonsterAttackState(this);
        ChangeState(idleState);
    }
}
