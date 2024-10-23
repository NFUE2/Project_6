using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterTrackState : MonsterBaseState
{
    public MonsterTrackState(MonsterStateMachine stateMachine) : base(stateMachine) { }
    Rigidbody2D rigidbody;

    public override void Enter()
    {
        base.Enter();
        //rigidbody = stateMachine.controller.rigidbody;
        StartAnimation(stateMachine.controller.animationData.move);
        rigidbody = stateMachine.controller.rigid;
    }

    public override void Exit()
    {
        base.Exit();
        rigidbody.velocity = Vector2.zero;
        StopAnimation(stateMachine.controller.animationData.move);
        //stateMachine.controller.
    }

    public override void HandleInput()
    {
        PlayerTracking();
        //Move();
    }

    void PlayerTracking()
    {
        Vector2 targetPos = stateMachine.controller.target.position;
        Vector2 myPos = stateMachine.controller.transform.position;

        float distance = TargetDistance();

        if (distance < stateMachine.controller.data.attackDistance)
            stateMachine.ChangeState(stateMachine.attackState);

        else if(!IsTrackable() || distance > stateMachine.controller.searchDistance)
            stateMachine.ChangeState(stateMachine.idleState);

        else
        {
            Move();
            Aim();
        }
    }

    void Move()
    {
        //rigidbody2D 사용으로 수정
        float speed = stateMachine.controller.data.moveSpeed;
        int direction = TargetDirection().x < 0 ? -1 : 1;

        rigidbody.velocity = new Vector2(direction * speed,rigidbody.velocity.y);
    }
}
