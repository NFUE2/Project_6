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
        // 아이템을 드랍할 것인지,
        // 인벤토리로 바로 아이템을 부여할 것인지,
        // 돈은 얼마나 줄 것인지,
        
    }
}