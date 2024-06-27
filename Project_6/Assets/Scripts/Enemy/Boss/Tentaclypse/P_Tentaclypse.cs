using System.Collections.Generic;
using UnityEngine;

public class P_Tentaclypse : P_BossMonster
{
    [SerializeField] private P_BossData tentaclypseData;
    private P_Dummy currentTarget;

    private void Start()
    {
        // �ɷ�ġ ����
        bossName = tentaclypseData.BossName;
        bossPower = tentaclypseData.BossPower;
        bossHp = tentaclypseData.BossHp;
        currentState = P_BossState.Idle;
        // �÷��̾� ���� ����
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
            var target = SetTarget();
            // Ȯ�ο� �ӽ��ڵ�
            currentTarget = target.GetComponent<P_Dummy>();
            Debug.Log($"{currentTarget.dummyName} �ֽ� ��");
            // Ȯ�ο� �ӽ��ڵ�
        }
    }

    protected override void AttackBehavior()
    {
        if (currentTarget != null)
        {
            // Ȯ�ο� �ӽ��ڵ�
            Debug.Log($"ũ�;�!! {currentTarget.dummyName}�� ��� �Ծ�����ڴ�!");
            currentState = P_BossState.Idle;
            // Ȯ�ο� �ӽ��ڵ�
        }
        else
        {
            Debug.Log("���� �ൿ ���� : ��� ����");
            currentState = P_BossState.Idle;
        }
    }

    protected override void DeadBehavior()
    {
        Debug.Log("���� ��� ��");
    }
}