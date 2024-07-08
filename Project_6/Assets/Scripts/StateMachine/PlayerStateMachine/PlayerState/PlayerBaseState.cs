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
        this.stateMachine = stateMachine; //���� ���¸ӽ��� ���
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
        //�Է� ���
        PlayerController input = stateMachine.Player.Input; //���¸ӽ� �÷��̾ �Էµ��

        //��������,��������,������������ �۵��ϴºκ�
        input.playerActions.Movement.canceled += OnMovementCanceled;

        input.playerActions.Run.started += OnRunStarted;
        input.playerActions.Run.canceled += OnRunCanceled;

        input.playerActions.Jump.started += OnJumpStarted;

        input.playerActions.Attack.performed += OnAttackPerfomed;
        input.playerActions.Attack.canceled += OnAttackCanceled;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        //�ٸ����·� ����� �� ������
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
        Move(); //�����Ӹ��� ������
    }

    //�̵�����������
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    //�޸��� �����Ҷ�
    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }
    
    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {

    }

    //������ �����Ҷ�
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }

    //���� ��������
    protected virtual void OnAttackPerfomed(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = true;
    }

    //���� ����ɶ�
    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = false;
    }

    //�ִϸ��̼� ����
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, true);
    }
    
    //�ִϸ��̼� ����
    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash,false);
    }

    //�÷��̾� ��ǲ�ý��ۿ��� �Է¹޾ƿ��ºκ�
    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    //�����̴� �κ�
    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        Move(movementDirection);
        Rotate(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        //����ī�޶� ����
        Vector3 forward = stateMachine.MainCamTransform.forward;
        Vector3 right = stateMachine.MainCamTransform.right;

        //Y���� ����
        forward.y = 0;
        right.y = 0;

        //���͵��� ����ȭ��Ŵ
        forward.Normalize();
        right.Normalize();

        //�Է����� �޾ƿ��ºκ��� ����
        //y�� Ű���� w,s , x�� a,dŰ 
        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    //������
    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed(); //���ǵ�
        // stateMachine.Player.ForceReceiver.Movement �߷¹޴ºκ�
        //�̵���
        stateMachine.Player.Controller.Move((direction * movementSpeed + stateMachine.Player.ForceReceiver.Movement ) * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        //���ǵ� ����
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    //ȸ��
    private void Rotate(Vector3 direction) 
    {
        if(direction != Vector3.zero) //�������� ������ �۵�
        {
            Transform playerTransform = stateMachine.Player.transform; //���� �÷��̾� ����
            Quaternion targetRotation = Quaternion.LookRotation(direction); //�ش� ������ �ٶ�
            //���� �������� ������ ����
            // stateMachine.RotationDamping ȸ�� ���ӵ� ��ġ�ε�
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
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0); //0�� ���̾�,���� �������� �ִϸ��̼� ����
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0); //��������

        // ��ȯ�ǰ� ���� �� && ���� �ִϸ��̼��� tag
        if(animator.IsInTransition(0) && nextInfo.IsTag(tag)) //��ȯ�ǰ��ִ��� �ƴ��� �˾ƺ����Լ�
        {
            return nextInfo.normalizedTime; //normalizedTime �ִϸ��̼� ���൵ 1�̸� ��� ���� 0�̸� ���۾���
        }
        // ��ȯ�ǰ� ���� ���� ��, ���� �ִϸ��̼��� tag
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
