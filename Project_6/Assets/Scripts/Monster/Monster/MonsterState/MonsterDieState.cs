using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MonsterDieState : MonsterBaseState
{
    public MonsterDieState(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartTriggerAnimation(stateMachine.controller.animationData.die);
    }
}
