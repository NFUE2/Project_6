using System.Collections.Generic;
using UnityEngine;

public class P_Tentaclypse : P_BossMonster
{
    [SerializeField] private P_BossData tentaclypseData;
    public GameObject target;
    private P_Dummy currentTarget;

    [Header("Attack Pattern")]
    public GameObject attackPatternManager;
    public GameObject razorObject;
    public GameObject allRoundAttackObject;
    public GameObject dispenser;

    private void Start()
    {
        // 능력치 설정
        bossName = tentaclypseData.BossName;
        bossPower = tentaclypseData.BossPower;
        bossHp = tentaclypseData.BossHp;
        currentState = P_BossState.Idle;
        // 패턴 추가 정보
        SetAttackPattern(attackPatternManager.GetComponent<P_TentaclypseRazorAttack>());
        SetAttackPattern(attackPatternManager.GetComponent<P_TentaclypseAllRoundShotAttack>());
        SetAttackPattern(attackPatternManager.GetComponent<P_TentaclypseRazorRainAttack>());
        SetAttackPattern(attackPatternManager.GetComponent<P_TentaclypseDispenserAttack>());
        // 플레이어 정보 수집
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        dummies = new List<GameObject>();
        foreach(GameObject player in players)
        {
            dummies.Add(player);
        }
    }

    protected override void IdleBehavior()
    {
        if(targetSettingTriggerTime >= targetSettingTime)
        {
            targetSettingTriggerTime = 0;
            if(target != null)
            {
                bool isTargetDead = currentTarget.IsDead();
                if (isTargetDead)
                {
                    dummies.Remove(target);
                }
            }
            target = SetTarget();
            // 확인용 임시코드
            currentTarget = target.GetComponent<P_Dummy>();
            Debug.Log($"{currentTarget.dummyName} 주시 중");
            // 확인용 임시코드
        }
    }

    protected override void AttackBehavior()
    {
        if (currentTarget != null)
        {
            int index = Random.Range(0,patterns.Count);
            patterns[index].ExecuteAttack();
            // 확인용 임시코드
            //Debug.Log($"크와앙!! {currentTarget.dummyName}를 잡아 먹어버리겠당!");
            currentState = P_BossState.Idle;
            // 확인용 임시코드
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
}