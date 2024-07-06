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
    // �÷��̾� ���� �޾ƿ���(Array or List�� ����)

    private void Start()
    {
        GetPlayers();
        SpawnBossMonster();
    }

    private void SpawnBossMonster() // ���� ��ȯ
    {
        spawnedBoss = Instantiate(bossMonster);
        boss = spawnedBoss.GetComponent<BossMonster>();
        
        if (boss != null)
        {
            Debug.Log("BossMonster ��ũ��Ʈ �ε� ����");
        }
        else
        {
            CreateBossMonsterStateMachine(boss);
        }
    }

    private void CreateBossMonsterStateMachine(BossMonster boss) // FSM ����
    {
        bossStateMachine = new BossMonsterStateMachine(boss);
    }

    private void GetPlayers() // �÷��̾� ���� �޾ƿ���
    {
        // List�� �����մϴ�. Player ��� �� List���� ��� �����ؾ� �ϹǷ�
        // Player ��ũ��Ʈ���� �̱��� �����Ͽ� Remove �ؾ��մϴ�.
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(p);
        }
    }
}
    // ���� ��ȯ �� FSM ����