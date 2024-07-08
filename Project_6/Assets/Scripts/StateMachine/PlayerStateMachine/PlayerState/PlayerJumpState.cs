using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        //점프파워 설정
        //stateMachine.JumpForce = stateMachine.Player.Data.AirData.JumpForce;

        //점프
        //stateMachine.Player.ForceReceiver.Jump(stateMachine.JumpForce);
        base.Enter();
        //점프 상태로변경
        //StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //점프 상태 해제
        //StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //떨어지고있을때
        if(stateMachine.Player.Controller.velocity.y <= 0)
        {
            //떨어지는 상태로 변경
            //stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}
