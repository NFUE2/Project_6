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
        //�̵����ӵ� 0�� ����
        stateMachine.MovementSpeedModifier = 0;
        base.Enter();
        //���� ���·� ����
        //StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //���� ���� ����
        //StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
}
