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

    public override void HandleInput()
    {
        base.HandleInput();

        if(GetNomalizeTime("Die",1.0f))
            stateMachine.controller.Disable();
    }
}
