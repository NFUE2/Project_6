using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAttack
{
    protected MonsterStateMachine stateMachine;

    protected MonsterAttack(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Attack();
}
