using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine; //현재 상태머신을 등록
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    protected virtual void AddInputActionCallbacks()
    {
        //입력 등록
        PlayerController input = stateMachine.Player.Input; //상태머신 플레이어에 입력등록

        //눌렀을때,누르는중,누르지않을때 작동하는부분
        input.playerActions.Movement.canceled += OnMovementCanceled;

        input.playerActions.Run.started += OnRunStarted;
        input.playerActions.Run.canceled += OnRunCanceled;

        input.playerActions.Jump.started += OnJumpStarted;

        input.playerActions.Attack.performed += OnAttackPerfomed;
        input.playerActions.Attack.canceled += OnAttackCanceled;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        //다른상태로 변경될 때 삭제됨
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;

        input.playerActions.Run.started -= OnRunStarted;
        input.playerActions.Run.canceled -= OnRunCanceled;

        input.playerActions.Jump.started -= OnJumpStarted;

        input.playerActions.Attack.performed -= OnAttackPerfomed;
        input.playerActions.Attack.canceled -= OnAttackCanceled;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        Move(); //프레임마다 움직임
    }

    //이동하지않을때
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    //달리기 시작할때
    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }
    
    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {

    }

    //점프를 시작할때
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }

    //공격 누르는중
    protected virtual void OnAttackPerfomed(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = true;
    }

    //공격 종료될때
    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = false;
    }

    //애니메이션 시작
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, true);
    }
    
    //애니메이션 종료
    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash,false);
    }

    //플레이어 인풋시스템에서 입력받아오는부분
    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    //움직이는 부분
    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        Move(movementDirection);
        Rotate(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        //메인카메라 방향
        Vector3 forward = stateMachine.MainCamTransform.forward;
        Vector3 right = stateMachine.MainCamTransform.right;

        //Y축은 제거
        forward.y = 0;
        right.y = 0;

        //벡터들을 정규화시킴
        forward.Normalize();
        right.Normalize();

        //입력으로 받아오는부분을 곱함
        //y는 키보드 w,s , x는 a,d키 
        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    //움직임
    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed(); //스피드
        // stateMachine.Player.ForceReceiver.Movement 중력받는부분
        //이동함
        stateMachine.Player.Controller.Move((direction * movementSpeed + stateMachine.Player.ForceReceiver.Movement ) * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        //스피드 설정
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    //회전
    private void Rotate(Vector3 direction) 
    {
        if(direction != Vector3.zero) //움직임이 있을때 작동
        {
            Transform playerTransform = stateMachine.Player.transform; //현재 플레이어 정보
            Quaternion targetRotation = Quaternion.LookRotation(direction); //해당 방향을 바라봄
            //구면 보간으로 서서히 돌음
            // stateMachine.RotationDamping 회전 가속도 수치인듯
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    //?
    protected void ForceMove()
    {
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime);
    }

    //
    protected float GetNoramalizedTime(Animator animator, string tag)
    {
        //
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0); //0은 레이어,현재 진행중인 애니메이션 정보
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0); //다음상태

        // 전환되고 있을 때 && 다음 애니메이션이 tag
        if(animator.IsInTransition(0) && nextInfo.IsTag(tag)) //전환되고있는지 아닌지 알아보는함수
        {
            return nextInfo.normalizedTime; //normalizedTime 애니메이션 진행도 1이면 모두 진행 0이면 시작안함
        }
        // 전환되고 있지 않을 때, 현재 애니메이션이 tag
        else if(!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
