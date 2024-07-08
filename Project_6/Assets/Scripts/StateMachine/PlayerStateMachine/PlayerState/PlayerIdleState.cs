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
        //�̵� ���ӵ� 0����
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        //�ִϸ��̼� idle���� ����
        //StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //�ִϸ��̼� idle���� ����
        //StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        //�����̸� �۵�
        if(stateMachine.MovementInput != Vector2.zero)
        {
            //�ȴ� ���·� ����
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
    }
}
