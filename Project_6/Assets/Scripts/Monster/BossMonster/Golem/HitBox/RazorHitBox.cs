using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorHitBox : HitBox
{
    private void OnEnable()
    {
        curDuration = 0f;
        duration = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name}�� �������� �ǰݵǾ����ϴ�.");
        }
    }
}
