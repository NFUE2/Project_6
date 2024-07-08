public class BossMonsterAttackState : BossMonsterBaseState
{
    public BossMonsterAttackState(BossMonsterStateMachine stateMachine) : base(stateMachine) { }

    // �������� ���� ���� �� ���� ����, 

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