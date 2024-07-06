using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossMonsterIdleState : BossMonsterBaseState
{
    public BossMonsterIdleState(BossMonsterStateMachine stateMachine) : base(stateMachine)
    {

    }
    // Ÿ�� �÷��̾� ����
    // �̵� ���

    public override void Enter()
    {
        base.Enter();
        // Idle State������ �ൿ ����
        SetTargetPlayer();
        float distance = Vector3.Distance(BossBattleManager.Instance.bossMonster.transform.position, BossBattleManager.Instance.targetPlayer.transform.position);
        if(distance > 4) 
        {
            StartCoroutine(Move());
        }
        // StartAnimation
    }

    public override void Exit()
    {
        base.Exit();
        // Idle State������ �ൿ ����
        // StopAnimation
    }

    private void SetTargetPlayer()
    {
        var players = BossBattleManager.Instance.players; // �̱������� �޾ƿ� �÷��̾� ����Ʈ
        int randInt = Random.Range(0, players.Count);
        BossBattleManager.Instance.targetPlayer = players[randInt];
    }

    private IEnumerator Move()
    {
        float elapsedTime = 0f;
        float moveDuration = Random.Range(1, 3);
        float moveSpeed = BossBattleManager.Instance.boss.moveSpeed;
        Vector3 bossPosition = BossBattleManager.Instance.spawnedBoss.transform.position;
        Vector3 targetPosition = BossBattleManager.Instance.targetPlayer.transform.position;
        while(elapsedTime < moveDuration)
        {
            float moveStep = moveSpeed * Time.deltaTime;
            if(bossPosition.x > targetPosition.x)
            {
                BossBattleManager.Instance.spawnedBoss.transform.Translate(-moveStep, 0, 0);
            }
            else if(bossPosition.x < targetPosition.x)
            {
                BossBattleManager.Instance.spawnedBoss.transform.Translate(moveStep, 0, 0);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}