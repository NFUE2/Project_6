using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorHitBox : HitBox
{
    public GameObject burningField;
    private void OnEnable()
    {
        curDuration = 0f;
        duration = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name}가 레이저에 피격되었습니다.");
            //if(collision.TryGetComponent<IDamagable>(out IDamagable P))
            //{
            //    float damage = BossBattleManager.Instance.boss.attackPower * 1.25f;
            //    P.TakeDamage(damage);
            //}
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            var field = Instantiate(burningField);
            field.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + 0.7f, collision.transform.position.z);
        }
    }
}
