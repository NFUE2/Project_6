using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MonsterCondition : MonoBehaviour,IDamagable
{
    MonsterController controller;

    //public event Action OnAlive;
    public event Action OnDie;
    public event Action OnSpawn;

    float curHP;

    private void Awake()
    {
        controller = GetComponent<MonsterController>();
        OnSpawn += SetHP;
    }

    private void OnEnable()
    {
        OnSpawn?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        curHP = Mathf.Clamp(curHP - damage, 0, controller.data.maxHP);

        if (curHP == 0)
        {
            Debug.Log("»ç¸Á");
            OnDie?.Invoke();
        }
    }

    private void SetHP()
    {
        curHP = controller.data.maxHP;
    }
}
