public class BossMonsterIdleState : BossMonsterBaseState
{
    public BossMonsterIdleState(BossMonsterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        // Idle State에서의 행동 시작
        // StartAnimation
    }

    public override void Exit()
    {
        base.Exit();
        // Idle State에서의 행동 종료
        // StopAnimation
    }
}