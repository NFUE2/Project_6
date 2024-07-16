using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter(); // State ���� �� �۵� �޼���
    public void Exit(); // State Ż�� �� �۵� �޼���
    public void HandleInput(); // State�� Input�� ������ �����ϴ� �޼���, �ʿ��� ��쿡�� ȣ�� ����(BaseState�� ���� ��)
}

public abstract class StateMachine
{
    protected IState currentState; // ���� State

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void HandleInput(bool input)
    {
        currentState?.HandleInput();
    }
}
