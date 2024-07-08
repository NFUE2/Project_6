using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        //이동 가속도 0설정
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        //애니메이션 idle상태 설정
        //StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //애니메이션 idle상태 해제
        //StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        //움직이면 작동
        if(stateMachine.MovementInput != Vector2.zero)
        {
            //걷는 상태로 변경
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
    }
}
