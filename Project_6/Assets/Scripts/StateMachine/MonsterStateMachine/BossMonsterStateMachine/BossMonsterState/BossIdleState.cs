using UnityEngine;

public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        BossBattleManager.Instance.isAttacking = false;
        SetTargetPlayer();
    }

    public override void Exit() 
    { 
        base.Exit();
    }

    private void SetTargetPlayer()
    {
        //var players = BossBattleManager.Instance.players; // 싱글톤으로 받아온 플레이어 리스트
        var players = TestGameManager.Instance.players; // 싱글톤으로 받아온 플레이어 리스트
        int randInt = Random.Range(0, players.Count);
        BossBattleManager.Instance.targetPlayer = players[randInt];
    }
}