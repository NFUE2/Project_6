public class BossAttackState : BossBaseState
{
    public BossAttackState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //BossBattleManager.Instance.ToggleIsAttacking();

        // 기존 코드
        BossBattleManager.Instance.attackController.SelectAttack();

        // 테스트 전용 코드
        //
        //.Instance.attackController.SelectAttack();
    }

    public override void Exit() 
    { 
        base.Exit();
    }
}