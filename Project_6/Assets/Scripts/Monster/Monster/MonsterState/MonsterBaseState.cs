using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseState : IState
{
    protected MonsterStateMachine stateMachine;

    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput() { }

    protected void StartAnimation(int animaotrHash)
    {
        //stateMachine.controller.animtor.SetBool(animaotrHash,true);
    }

    protected void StopAnimation(int animaotrHash)
    {
        //stateMachine.controller.animtor.SetBool(animaotrHash, false);
    }
}
