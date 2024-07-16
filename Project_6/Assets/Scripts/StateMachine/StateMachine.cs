using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter(); // State 진입 시 작동 메서드
    public void Exit(); // State 탈출 시 작동 메서드
    public void HandleInput(); // State별 Input의 동작을 변경하는 메서드, 필요한 경우에만 호출 가능(BaseState에 선언 시)
}

public abstract class StateMachine
{
    protected IState currentState; // 현재 State

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
