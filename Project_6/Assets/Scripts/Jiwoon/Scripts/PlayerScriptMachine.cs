using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerState _currentState;

    void Update()
    {
        HandleState(_currentState);
    }

    private void HandleState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                // 아이들
                break;
            case PlayerState.Walking:
                // 걷기
                break;
            case PlayerState.Running:
                // 달리기
                break;
            case PlayerState.Jumping:
                // 점프
                break;
            case PlayerState.Attacking:
                // 공격
                break;
            case PlayerState.UsingSkill:
                // 스킬                  
                break;
            case PlayerState.Dead:
                // 죽음
                break;
        }
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState = newState;
    }
}
