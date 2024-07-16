using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterTrackState : MonsterBaseState
{
    public MonsterTrackState(MonsterStateMachine stateMachine) : base(stateMachine) { }

    //Rigidbody2D rigidbody;
    public override void Enter()
    {
        //rigidbody = stateMachine.controller.rigidbody;
        StartAnimation(stateMachine.controller.animationData.move);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.controller.animationData.move);
    }

    public override void HandleInput()
    {
        PlayerTracking();
        Move();
    }

    void PlayerTracking()
    {
        Vector2 targetPos = stateMachine.controller.target.position;
        Vector2 myPos = stateMachine.controller.transform.position;

        float distance = TargetDistance();

        if(distance > stateMachine.controller.data.searchDistance)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
        else if (distance < stateMachine.controller.data.attackDistance)
        {
            stateMachine.ChangeState(stateMachine.attackState);
        }

        stateMachine.controller.transform.localScale = TargetDirection().x < 0 ? new Vector3(-1,1,1) : Vector3.one;
    }

    void Move()
    {
        //rigidbody2D 사용으로 수정
        float speed = stateMachine.controller.data.moveSpeed;
        stateMachine.controller.transform.Translate(TargetDirection() * speed * Time.deltaTime);
    }
}
