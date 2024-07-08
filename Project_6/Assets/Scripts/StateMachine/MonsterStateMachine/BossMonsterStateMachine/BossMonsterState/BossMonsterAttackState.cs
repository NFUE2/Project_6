public class BossMonsterAttackState : BossMonsterBaseState
{
    public BossMonsterAttackState(BossMonsterStateMachine stateMachine) : base(stateMachine) { }

    // 랜덤으로 패턴 결정 후 패턴 수행, 

    public override void Enter()
    {
        base.Enter();
        BossBattleManager.Instance.attackController.SelectAttack();
    }

    public override void Exit()
    {
        base.Exit();
    }
}