using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : Singleton<BossBattleManager>
{
    public GameObject bossMonster;
    public GameObject spawnedBoss;
    public BossMonster boss;
    public List<GameObject> players;
    public GameObject targetPlayer;
    public BossMonsterStateMachine bossStateMachine;
    // 플레이어 정보 받아오기(Array or List로 관리)

    private void Start()
    {
        GetPlayers();
        SpawnBossMonster();
    }

    private void SpawnBossMonster() // 보스 소환
    {
        spawnedBoss = Instantiate(bossMonster);
        boss = spawnedBoss.GetComponent<BossMonster>();
        
        if (boss != null)
        {
            Debug.Log("BossMonster 스크립트 로드 에러");
        }
        else
        {
            CreateBossMonsterStateMachine(boss);
        }
    }

    private void CreateBossMonsterStateMachine(BossMonster boss) // FSM 생성
    {
        bossStateMachine = new BossMonsterStateMachine(boss);
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