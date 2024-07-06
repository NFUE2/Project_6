using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterBaseState : MonoBehaviour, IState
{
    protected BossMonsterStateMachine stateMachine;
    public BossMonsterBaseState(BossMonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        // State ���� ��
    }

    public virtual void Exit()
    {
        // State Ż�� ��
    }

    public virtual void HandleInput()
    {
        // ���� ���ʹ� ��� ����
    }

    // ���� ���� ���� ���� �޼��� �߰� ���� �ʿ信 ���� virtual �����Ͽ� override ���
    // example
    // Move() �̵�
    // SetTarget() Ÿ�� �÷��̾� ����
    // Rotate() Ÿ�� �÷��̾� �������� Sprite ȸ��
}
