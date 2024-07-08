using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier; //달리기 가속도 설정
        base.Enter();
        //달리기 상태로 변경
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //달리기 상태로 해제
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        base.OnRunCanceled(context);

        stateMachine.ChangeState(stateMachine.IdleState);
    }
}
