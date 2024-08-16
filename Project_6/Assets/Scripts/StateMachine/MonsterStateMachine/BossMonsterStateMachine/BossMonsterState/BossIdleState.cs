using UnityEngine;

public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        // ���� �ڵ�
        BossBattleManager.Instance.isAttacking = false;
        if (BossBattleManager.Instance.targetPlayer != null)
        {
            SetTargetPlayer();
        }


        // �׽�Ʈ ���� �ڵ�
        //BossTestManager.Instance.isAttacking = false;
        //(BossTestManager.Instance.targetPlayer != null)
        //{
        //    SetTargetPlayer();
        //}
    }

    public override void Exit() 
    { 
        base.Exit();
    }

    private void SetTargetPlayer()
    {
        // ���� �ڵ�
        var players = BossBattleManager.Instance.players; // �̱������� �޾ƿ� �÷��̾� ����Ʈ



        // �׽�Ʈ ���� �ڵ�
        //var players = BossTestManager.Instance.players;

        //var players = TestGameManager.Instance.players; // �̱������� �޾ƿ� �÷��̾� ����Ʈ(����)


        int randInt = Random.Range(0, players.Count);

        //���� �ڵ�
        BossBattleManager.Instance.targetPlayer = players[randInt];
    
    
        // �׽�Ʈ ���� �ڵ�
        //BossTestManager.Instance.targetPlayer = players[randInt];
    }
}