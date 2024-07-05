using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterStateMachine : StateMachine
{
    public BossMonster BossMonster { get; }

    public float MovementSpeed { get; set; }
    public bool IsAttacking { get; set; }

    public BossMonsterIdleState IdleState { get; }
    public BossMonsterAttackState AttackState { get; }
    public BossMonsterDieState DieState { get; }

    public BossMonsterStateMachine(BossMonster bossMonster)
    {
        this.BossMonster = bossMonster;

        IdleState = new BossMonsterIdleState(this);
        AttackState = new BossMonsterAttackState(this);
        DieState = new BossMonsterDieState(this);

        // MovementSpeed -> bossData에서 불러오기
    }
}
