public class BossMonsterIdleState : BossMonsterBaseState
{
    public BossMonsterIdleState(BossMonsterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        // Idle State������ �ൿ ����
        // StartAnimation
    }

    public override void Exit()
    {
        base.Exit();
        // Idle State������ �ൿ ����
        // StopAnimation
    }
}