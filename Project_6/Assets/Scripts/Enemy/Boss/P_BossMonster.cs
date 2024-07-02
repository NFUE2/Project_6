using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum P_BossState
{
    Idle,
    Attacking,
    Fainting,
    Dead
}

public interface IAttackPattern
{
    public void ExecuteAttack();
}

[RequireComponent(typeof(PhotonView))]
public abstract class P_BossMonster : MonoBehaviourPun, P_IDamagable,IPunObservable
{
    public string bossName;
    public float bossPower;
    public float maxHp;
    public float bossHp;
    public List<GameObject> players;
    protected P_BossState currentState;
    protected List<IAttackPattern> patterns = new List<IAttackPattern>();

    public float targetSettingTime;
    public float targetSettingTriggerTime;
    public float attackCoolDown;
    public float attackTriggerTime;

    Image hpui;

    private void Awake()
    {
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            players.Add(p);

        hpui = GameObject.Find("BossCurrent_HP").GetComponent<Image>();
    }

    private void Update()
    {
        HandleState();
        attackTriggerTime += Time.deltaTime;
        targetSettingTriggerTime += Time.deltaTime;
        if (currentState == P_BossState.Idle && attackTriggerTime >= attackCoolDown)
        {
            attackTriggerTime = 0;
            currentState = P_BossState.Attacking;
        }
    }

    public void SetAttackPattern(IAttackPattern newAttackPattern)
    {
        patterns.Add(newAttackPattern);
    }

    void HandleState()
    {
        //Debug.Log($"HandleState 실행 중 상태 : {currentState.ToString()}");
        switch (currentState)
        {
            case P_BossState.Idle:
                IdleBehavior();
                break;
            case P_BossState.Attacking:
                AttackBehavior();
                break;
            case P_BossState.Dead:
                DeadBehavior();
                break;
        }
    }

    protected abstract void IdleBehavior();
    protected abstract void AttackBehavior();
    protected abstract void DeadBehavior();

    public void TakeDamage(float damage)
    {
        bossHp -= damage;
        hpui.fillAmount = bossHp / maxHp;

        if (bossHp <= 0)
            currentState = P_BossState.Dead;
    }
    public GameObject SetTarget()
    {
        int index = Random.Range(0, players.Count);
        return players[index];
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(hpui.fillAmount);
        }
        else
        {
            hpui.fillAmount = (float)stream.ReceiveNext();
        }
    }
}
