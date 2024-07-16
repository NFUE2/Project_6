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
        stateMachine.controller.animator.SetBool(animaotrHash, true);
    }
    protected void StartTriggerAnimation(int animaotrHash)
    {
        stateMachine.controller.animator.SetTrigger(animaotrHash);
    }

    protected void StopAnimation(int animaotrHash)
    {
        stateMachine.controller.animator.SetBool(animaotrHash, false);
    }

    public float TargetDistance()
    {
        Vector2 targetPos = stateMachine.controller.target.position;
        Vector2 myPos = stateMachine.controller.transform.position;

        return Vector2.Distance(targetPos, myPos);
    }

    public Vector2 TargetDirection()
    {
        Vector2 targetPos = stateMachine.controller.target.position;
        Vector2 myPos = stateMachine.controller.transform.position;

        return (targetPos - myPos).normalized;
    }
}