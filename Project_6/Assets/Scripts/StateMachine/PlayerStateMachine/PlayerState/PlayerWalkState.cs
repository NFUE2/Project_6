using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier; //가속도 설정
        base.Enter();
        //이동 상태 설정
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //이동 상태 해제
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    //달리기 시작
    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        //달리기 상태로 변경
        stateMachine.ChangeState(stateMachine.RunState);
    }
}
