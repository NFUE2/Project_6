using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public abstract class P_BossMonster : MonoBehaviour
{
    public string bossName;
    public float bossPower;
    public float bossHp;
    public List<GameObject> dummies;
    protected P_BossState currentState;
    protected List<IAttackPattern> patterns = new List<IAttackPattern>();

    public float targetSettingTime;
    public float targetSettingTriggerTime;
    public float attackCoolDown;
    public float attackTriggerTime;

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
        Debug.Log($"HandleState 실행 중 상태 : {currentState.ToString()}");
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
        if (bossHp <= 0)
        {
            currentState = P_BossState.Dead;
        }
    }

    public GameObject SetTarget()
    {
        int index = Random.Range(0, dummies.Count);
        return dummies[index];
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
