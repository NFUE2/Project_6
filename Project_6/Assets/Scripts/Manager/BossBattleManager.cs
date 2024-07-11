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
    public Animator bossAnimator;
    public BossStateMachine bossStateMachine;

    private float attackCoolDown = 3f;
    private float curCoolDown = 0f;
    private bool isFirst = true;
    public bool isAttacking;
    // 플레이어 정보 받아오기(Array or List로 관리)

    private void Start()
    {
        GetPlayers();
        SpawnBossMonster();
        isAttacking = false;
    }

    private void Update()
    {
        if(spawnedBoss != null && bossStateMachine != null)
        {
            
            if(players != null)
            {
                if(boss.currentHp <= 0)
                {
                    if(bossStateMachine.GetCurrentState() != bossStateMachine.DieState)
                    {
                        bossStateMachine.ChangeState(bossStateMachine.DieState);
                    }
                }
                else
                {
                    if (isAttacking == false)
                    {
                        curCoolDown += Time.deltaTime;
                    }
                    if (isFirst)
                    {
                        isFirst = false;
                        bossStateMachine.ChangeState(bossStateMachine.IdleState);
                    }
                    else
                    {
                        if (curCoolDown >= attackCoolDown && isAttacking == false)
                        {
                            bossStateMachine.ChangeState(bossStateMachine.AttackState);
                            curCoolDown = 0;
                        }
                    }
                }
            }
        }
    }

    private void SpawnBossMonster() // 보스 소환
    {
        spawnedBoss = Instantiate(bossMonster, transform.position,Quaternion.identity);
        boss = spawnedBoss.GetComponent<BossMonster>();
        attackController = spawnedBoss.GetComponent<BossAttackController>();
        if (boss != null && bossStateMachine == null)
        {
            CreateBossMonsterStateMachine(boss);  
        }
        else
        {
            Debug.Log("BossMonster 스크립트 로드 에러");
        }
    }

    private void CreateBossMonsterStateMachine(BossMonster boss) // FSM 생성
    {
        bossStateMachine = spawnedBoss.GetComponent<BossStateMachine>();
        bossAnimator = spawnedBoss.GetComponent<Animator>();
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

    public void ToggleIsAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            Debug.Log($"토글 {isAttacking}");
        }
        else if(!isAttacking)
        {
            isAttacking = true;
            Debug.Log($"토글 {isAttacking}");
        }
    }

    public void DestroyBoss()
    {
        Destroy(bossMonster);
    }
}
    // 보스 소환 및 FSM 생성