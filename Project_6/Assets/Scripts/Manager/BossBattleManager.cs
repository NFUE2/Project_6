using Photon.Pun;
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
    public float distanceToTarget;
    public Animator bossAnimator;
    public BossStateMachine bossStateMachine;
    public GameObject[] bossEndObject;

    private float attackCoolDown = 6f;
    private float curCoolDown = 0f;
    private bool isFirst = true;
    public bool isAttacking;
    // �÷��̾� ���� �޾ƿ���(Array or List�� ����)

    public override void Awake()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) Destroy(gameObject);
        base.Awake();
    }

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
                if (targetPlayer != null)
                {
                    distanceToTarget = CalcDistance();
                }
                else if (targetPlayer == null)
                {
                    distanceToTarget = -1;
                }

                if (boss.currentHp <= 0)
                {
                    if(bossStateMachine.GetCurrentState() != bossStateMachine.DieState)
                    {
                        bossStateMachine.ChangeState(bossStateMachine.DieState);
                    }
                    return;
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

    public float CalcDistance()
    {
        return Vector3.Distance(targetPlayer.transform.position, spawnedBoss.transform.position);
    }

    private void SpawnBossMonster() // ���� ��ȯ
    {
        //spawnedBoss = Instantiate(bossMonster, transform.position,Quaternion.identity);
        spawnedBoss = PhotonNetwork.Instantiate("Boss/" + bossMonster.name, transform.position, Quaternion.identity);
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


        //players = TestGameManager.instance.players;(����)
    }

    public void ToggleIsAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            Debug.Log($"��� {isAttacking}");
        }
        else if(!isAttacking)
        {
            isAttacking = true;
            Debug.Log($"��� {isAttacking}");
        }
    }

    public void DestroyBoss()
    {
        Debug.Log(1);
        //Destroy(bossMonster);
        spawnedBoss.SetActive(false);
        foreach(GameObject g in bossEndObject)
            g.SetActive(!g.activeInHierarchy);

        GameManager.instance.cam.target = GameManager.instance.player.transform;
    }
}
    // ���� ��ȯ �� FSM ����