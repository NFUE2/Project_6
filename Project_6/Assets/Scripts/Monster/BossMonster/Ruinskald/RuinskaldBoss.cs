using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinskaldBoss : BossMonster
{
    public CharacterDataSO ruinskaldData;

    private void Start()
    {
        maxHp = ruinskaldData.maxHP * SetMultiHP();
        currentHp = maxHp;
        attackPower = ruinskaldData.attackDamage;
        defensePower = ruinskaldData.defence;
        moveSpeed = ruinskaldData.moveSpeed;
    }
}
