using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : Singleton<BossBattleManager>
{
    public GameObject[] bossMonsters = new GameObject[2];
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
        //if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) Destroy(gameObject);
        base.Awake();
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
                    if (boss.currentHp <= 0)
                    {
                        if (bossStateMachine.GetCurrentState() != bossStateMachine.DieState)
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
                else if (targetPlayer == null)
                {
                    distanceToTarget = -1;
                }
            }
        }
    }

    public float CalcDistance()
    {
        return Vector3.Distance(targetPlayer.transform.position, spawnedBoss.transform.position);
    }

    public void SpawnBossMonster(int index,Vector3 spawnPos) // ���� ��ȯ
    {
        GetPlayers();
        isAttacking = false;
        //spawnedBoss = Instantiate(bossMonster, transform.position,Quaternion.identity);
        
        spawnedBoss = PhotonNetwork.Instantiate(bossMonsters[index].name, spawnPos, Quaternion.identity);
        switch (index)
        {
            case 0:
                attackCoolDown = 6;
                break;
            case 1:
                attackCoolDown = 3;
                break;  
        }
        boss = spawnedBoss.GetComponent<BossMonster>();
        attackController = spawnedBoss.GetComponent<BossAttackController>();
        if (boss != null)
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
        //foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        //{
        //    players.Add(p);
        //}


        players = GameManager.instance.players;
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
        if(PhotonNetwork.IsMasterClient) PhotonNetwork.Destroy(spawnedBoss);
        //spawnedBoss.SetActive(false);

        foreach(GameObject g in bossEndObject)
            g.SetActive(true);

        //GameManager.instance.cam.target = GameManager.instance.player.transform;
    }
}
    // ���� ��ȯ �� FSM ����