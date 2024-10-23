using System.Collections.Generic;
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
        List<GameObject> targets = new List<GameObject>();
        foreach (var p in players)
        {
            PlayerCondition pc = p.GetComponent<PlayerCondition>();
            if (pc != null) 
            {
                if(pc.currentHealth > 0)
                {
                    targets.Add(p);
                }
            }
        }



        // �׽�Ʈ ���� �ڵ�
        //var players = BossTestManager.Instance.players;

        //var players = TestGameManager.Instance.players; // �̱������� �޾ƿ� �÷��̾� ����Ʈ(����)


        int randInt = Random.Range(0, targets.Count);
        stateMachine.GetComponent<BossMonster>().SetTarget(randInt);
        //���� �ڵ�
        //BossBattleManager.Instance.targetPlayer = targets[randInt];
    
    
        // �׽�Ʈ ���� �ڵ�
        //BossTestManager.Instance.targetPlayer = players[randInt];
    }
}