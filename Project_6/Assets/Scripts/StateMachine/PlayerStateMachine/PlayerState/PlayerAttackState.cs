using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        //이동가속도 0로 설정
        stateMachine.MovementSpeedModifier = 0;
        base.Enter();
        //공격 상태로 변경
        //StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //공격 상태 해제
        //StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
}
