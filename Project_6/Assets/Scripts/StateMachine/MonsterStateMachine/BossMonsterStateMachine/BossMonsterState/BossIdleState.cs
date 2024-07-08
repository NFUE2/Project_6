using UnityEngine;

public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        SetTargetPlayer();
    }

    public override void Exit() 
    { 
        base.Exit();
    }

    private void SetTargetPlayer()
    {
        Debug.Log("Ÿ�� ����");
        var players = BossBattleManager.Instance.players; // �̱������� �޾ƿ� �÷��̾� ����Ʈ
        int randInt = Random.Range(0, players.Count);
        BossBattleManager.Instance.targetPlayer = players[randInt];
    }
}