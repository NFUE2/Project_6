using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHitBox : HitBox
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name}�� ������ �ǰݵǾ����ϴ�.");
        }
    }
}
