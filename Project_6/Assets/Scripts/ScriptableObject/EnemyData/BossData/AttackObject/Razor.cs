using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razor : MonoBehaviour
{
    private bool isAttackable = false;
    private float stateChangeTime = 1;
    private float stateChangeTriggerTime;
    private float objectDestroyTime = 0.5f;
    private float objectDestroyTriggerTime;
    private GameObject boss;
    private P_Tentaclypse tentaclypse;
    SpriteRenderer spriteRenderer;
    BoxCollider2D razorCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boss = GameObject.FindGameObjectWithTag("Boss");
        tentaclypse = boss.GetComponent<P_Tentaclypse>();
        razorCollider = gameObject.GetComponent<BoxCollider2D>();
        stateChangeTriggerTime = 0;
        objectDestroyTriggerTime = 0;
    }

    void Update()
    {
        stateChangeTriggerTime += Time.deltaTime;
        if(stateChangeTriggerTime >= stateChangeTime)
        {
            isAttackable = true;
            spriteRenderer.color = new Color(255, 0, 0, 255);
        }
        if(isAttackable)
        {
            razorCollider.enabled = true;
            objectDestroyTriggerTime += Time.deltaTime;
            if(objectDestroyTriggerTime >= objectDestroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isAttackable)
        {
            if (collision.CompareTag("Player"))
            {
                P_Dummy dummy = collision.GetComponent<P_Dummy>();
                Debug.Log($"{dummy.dummyName} ÇÇ°Ý!");
                dummy.TakeDamage(tentaclypse.bossPower);
            }
        }
    }
}
