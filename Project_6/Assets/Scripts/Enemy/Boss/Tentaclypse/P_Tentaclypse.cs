using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class P_Tentaclypse : P_BossMonster
{
    [SerializeField] private P_BossData tentaclypseData;
    public GameObject target;
    private P_PlayerCondition currentTarget;

    [Header("Attack Pattern")]
    public GameObject attackPatternManager;
    public GameObject razorObject;
    public GameObject allRoundAttackObject;
    public GameObject dispenser;

    private PhotonView pv;

    private void Start()
    {
        // 능력치 설정
        bossName = tentaclypseData.BossName;
        bossPower = tentaclypseData.BossPower;
        bossHp = tentaclypseData.BossHp;
        maxHp = tentaclypseData.BossHp;
        currentState = P_BossState.Idle;
        // 패턴 추가 정보
        SetAttackPattern(attackPatternManager.GetComponent<P_TentaclypseRazorAttack>());
        SetAttackPattern(attackPatternManager.GetComponent<P_TentaclypseAllRoundShotAttack>());
        SetAttackPattern(attackPatternManager.GetComponent<P_TentaclypseRazorRainAttack>());
        SetAttackPattern(attackPatternManager.GetComponent<P_TentaclypseDispenserAttack>());
        // 플레이어 정보 수집
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        base.players = new List<GameObject>();
        foreach(GameObject player in players)
        {
            base.players.Add(player);
        }

        pv = GetComponent<PhotonView>();
    }

    protected override void IdleBehavior()
    {
        if(targetSettingTriggerTime >= targetSettingTime)
        {
            targetSettingTriggerTime = 0;
            target = SetTarget();
            currentTarget = target.GetComponent<P_PlayerCondition>();
        }
    }

    protected override void AttackBehavior()
    {
        if (currentTarget != null)
        {
            //int index = Random.Range(0,patterns.Count);
            //patterns[index].ExecuteAttack();
            //currentState = P_BossState.Idle;
            if(PhotonNetwork.IsMasterClient)
                pv.RPC("AttackRPC", RpcTarget.MasterClient);
        }
        else
        {
            Debug.Log("공격 행동 에러 : 대상 없음");
            currentState = P_BossState.Idle;
        }
    }

    protected override void DeadBehavior()
    {
        Debug.Log("느왕 쥬금 ㅠ");
    }

    [PunRPC]
    private void AttackRPC()
    {
        int index = Random.Range(0, patterns.Count);
        patterns[index].ExecuteAttack();
        currentState = P_BossState.Idle;
    }
}