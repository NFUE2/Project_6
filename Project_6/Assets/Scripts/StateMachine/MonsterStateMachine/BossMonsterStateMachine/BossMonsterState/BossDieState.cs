using System.Diagnostics;

public class BossDieState : BossBaseState
{
    public BossDieState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        DropReward();
        BossBattleManager.Instance.DestroyBoss();
    }

    public override void Exit() 
    { 
        base.Exit();
    }

    private void DropReward()
    {
        // �������� ����� ������,
        // �κ��丮�� �ٷ� �������� �ο��� ������,
        // ���� �󸶳� �� ������,
    }
}