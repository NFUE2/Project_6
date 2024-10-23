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

        // 기존 코드
        BossBattleManager.Instance.isAttacking = false;
        if (BossBattleManager.Instance.targetPlayer != null)
        {
            SetTargetPlayer();
        }


        // 테스트 전용 코드
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
        // 기존 코드
        var players = BossBattleManager.Instance.players; // 싱글톤으로 받아온 플레이어 리스트
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



        // 테스트 전용 코드
        //var players = BossTestManager.Instance.players;

        //var players = TestGameManager.Instance.players; // 싱글톤으로 받아온 플레이어 리스트(복원)


        int randInt = Random.Range(0, targets.Count);
        stateMachine.GetComponent<BossMonster>().SetTarget(randInt);
        //기존 코드
        //BossBattleManager.Instance.targetPlayer = targets[randInt];
    
    
        // 테스트 전용 코드
        //BossTestManager.Instance.targetPlayer = players[randInt];
    }
}