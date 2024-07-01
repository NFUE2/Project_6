using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Razor : MonoBehaviour
{
    private bool isAttackable = false;
    private float stateChangeTime = 0.75f;
    private float stateChangeTriggerTime;
    private float objectDestroyTime = 0.25f;
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
                //Destroy(gameObject);
                DestroyObject();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isAttackable)
        {
            if (collision.CompareTag("Player"))
            {
                P_PlayerCondition player = collision.GetComponent<P_PlayerCondition>();
                player.TakeDamage(tentaclypse.bossPower);
                DestroyObject();
            }
            else if(collision.TryGetComponent(out P_Sheild shield))
            {
                shield.TakeDamage(tentaclypse.bossPower);
                DestroyObject();
            }
        }
    }

    private void DestroyObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
