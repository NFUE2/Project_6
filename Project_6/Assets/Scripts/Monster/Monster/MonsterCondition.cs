using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCondition : MonoBehaviour,IDamagable
{
    MonsterController controller;

    public event Action OnAlive;

    float maxHP,curHP;

    private void Awake()
    {
        controller = GetComponent<MonsterController>();
        curHP = maxHP = controller.data.maxHP;

        //OnDie += Die;
    }

    public void TakeDamage(float damage)
    {
        curHP = Mathf.Clamp(curHP - damage, 0, maxHP);

        //if(curHP == 0)
            //OnDie?.Invoke();
    }

    //void Die()
    //{
    //    enabled = false;
    //}
}
