using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    //���� Ŭ������ �����ڸ� ȣ���ϴµ�
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        //���¸ӽ����� ����
        //StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //���¸ӽ� ����
        //StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(stateMachine.IsAttacking) //���� �������� �۵�
        {
            OnAttack();
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //�ٴڿ� �ȴ��, y�� ��ȭ���� �߷º��� Ŭ��
        //https://docs.unity3d.com/ScriptReference/CharacterController-velocity.html
        if (!stateMachine.Player.Controller.isGrounded && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            //�������� ���� �۵�
            //stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    //������ ����
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput == Vector2.zero) return; //�ȿ����̸� ����

        stateMachine.ChangeState(stateMachine.IdleState); //idle�� ���º���

        base.OnMovementCanceled(context);
    }

    //��������
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        //�������·κ���
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    //���ݻ��·� ����
    protected void OnAttack()
    {
        //stateMachine.ChangeState(stateMachine.ComboAttackState);
    }

}
