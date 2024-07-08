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
                // ���̵�
                break;
            case PlayerState.Walking:
                // �ȱ�
                break;
            case PlayerState.Running:
                // �޸���
                break;
            case PlayerState.Jumping:
                // ����
                break;
            case PlayerState.Attacking:
                // ����
                break;
            case PlayerState.UsingSkill:
                // ��ų                  
                break;
            case PlayerState.Dead:
                // ����
                break;
        }
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState = newState;
    }
}
