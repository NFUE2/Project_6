using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    //상위 클래스의 생성자를 호출하는듯
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        //상태머신으로 진입
        //StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //상태머신 해제
        //StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(stateMachine.IsAttacking) //공격 눌럿을때 작동
        {
            OnAttack();
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //바닥에 안닿고, y축 변화량이 중력보다 클때
        //https://docs.unity3d.com/ScriptReference/CharacterController-velocity.html
        if (!stateMachine.Player.Controller.isGrounded && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            //떨어지는 상태 작동
            //stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    //움직임 종료
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput == Vector2.zero) return; //안움직이면 리턴

        stateMachine.ChangeState(stateMachine.IdleState); //idle로 상태변경

        base.OnMovementCanceled(context);
    }

    //점프시작
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        //점프상태로변경
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    //공격상태로 변경
    protected void OnAttack()
    {
        //stateMachine.ChangeState(stateMachine.ComboAttackState);
    }

}
