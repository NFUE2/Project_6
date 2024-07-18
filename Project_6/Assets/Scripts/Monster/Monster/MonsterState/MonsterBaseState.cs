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
        MonsterController controller = stateMachine.controller;
        Transform myTransform = controller.transform;

        Vector2 targetPos = controller.target.position;
        Vector2 offsetPos = controller.offsetPos;

        if (myTransform.localScale.x == -1)
            offsetPos = new Vector2(-offsetPos.x, offsetPos.y);

        Vector2 myPos = ((Vector2)myTransform.position + offsetPos);

        return Vector2.Distance(targetPos, myPos);
    }

    public Vector2 TargetDirection()
    {
        Vector2 targetPos = stateMachine.controller.target.position;
        Vector2 myPos = stateMachine.controller.transform.position;

        return (targetPos - (myPos + stateMachine.controller.offsetPos)).normalized;
    }

    public void Aim()
    {
        stateMachine.controller.isRight = !(stateMachine.controller.data.isRight ^ TargetDirection().x > 0) ? true : false;
        //Debug.Log(stateMachine.controller.data.isRight ^ TargetDirection().x > 0);
        stateMachine.controller.transform.localScale = stateMachine.controller.isRight ? Vector3.one : new Vector3(-1, 1, 1);
    }
}