public class BossDieState : BossBaseState
{
    public BossDieState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        DropReward();
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