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

    private float attackCoolDown = 5f;
    private float curCoolDown = 0f;
    private bool isFirst = true;
    public bool isAttacking = false;
    // �÷��̾� ���� �޾ƿ���(Array or List�� ����)

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
                    isFirst = false;
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

    private void SpawnBossMonster() // ���� ��ȯ
    {
        spawnedBoss = Instantiate(bossMonster);
        boss = spawnedBoss.GetComponent<BossMonster>();
        attackController = spawnedBoss.GetComponent<BossAttackController>();
        if (boss != null && bossStateMachine == null)
        {
            CreateBossMonsterStateMachine(boss);  
        }
        else
        {
            Debug.Log("BossMonster ��ũ��Ʈ �ε� ����");
        }
    }

    private void CreateBossMonsterStateMachine(BossMonster boss) // FSM ����
    {
        bossStateMachine = spawnedBoss.GetComponent<BossStateMachine>();
        bossAnimator = spawnedBoss.GetComponent<Animator>();
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