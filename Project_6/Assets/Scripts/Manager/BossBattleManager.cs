using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : Singleton<BossBattleManager>
{
    public GameObject bossMonster;
    public GameObject spawnedBoss;
    public BossMonster boss;
    public BossAttackController attackController;
    public List<GameObject> players;
    public GameObject targetPlayer;
    public BossStateMachine bossStateMachine;

    private float attackCoolDown = 3f;
    private float curCoolDown = 3f;
    private bool isFirst = true;
    public bool isAttacking = false;
    // 플레이어 정보 받아오기(Array or List로 관리)

    private void Start()
    {
        GetPlayers();
        SpawnBossMonster();
    }

    private void Update()
    {
        if(spawnedBoss != null && bossStateMachine != null)
        {
            
            if(players != null)
            {
                curCoolDown += Time.deltaTime;
                if(isFirst)
                {
                    Debug.Log("BSM NULL 아님");
                    isFirst = false;
                    Debug.Log($"{bossStateMachine.IdleState}");
                    bossStateMachine.ChangeState(bossStateMachine.IdleState);
                }
                else
                {
                    if(curCoolDown >= attackCoolDown)
                    {
                        bossStateMachine.ChangeState(bossStateMachine.AttackState);
                        curCoolDown = 0;
                    }
                }
            }
        }
    }

    private void SpawnBossMonster() // 보스 소환
    {
        spawnedBoss = Instantiate(bossMonster);
        boss = spawnedBoss.GetComponent<BossMonster>();
        attackController = spawnedBoss.GetComponent<BossAttackController>();
        if (boss != null && bossStateMachine == null)
        {
            CreateBossMonsterStateMachine(boss);
            Debug.Log("BSM 생성");   
        }
        else
        {
            Debug.Log("BossMonster 스크립트 로드 에러");
        }
    }

    private void CreateBossMonsterStateMachine(BossMonster boss) // FSM 생성
    {
        bossStateMachine = spawnedBoss.GetComponent<BossStateMachine>();
    }

    private void GetPlayers() // 플레이어 정보 받아오기
    {
        // List로 관리합니다. Player 사망 시 List에서 요소 제거해야 하므로
        // Player 스크립트에서 싱글톤 접근하여 Remove 해야합니다.
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(p);
        }
    }
}
    // 보스 소환 및 FSM 생성