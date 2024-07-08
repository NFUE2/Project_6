public class BossAttackState : BossBaseState
{
    public BossAttackState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        BossBattleManager.Instance.isAttacking = true;
        BossBattleManager.Instance.attackController.SelectAttack();
    }

    public override void Exit() 
    { 
        base.Exit();
    }
}