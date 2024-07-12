using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHitBox : HitBox
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name}가 스윙에 피격되었습니다.");
            if (collision.TryGetComponent<IDamagable>(out IDamagable P))
            {
                float damage = BossBattleManager.Instance.boss.attackPower * 0.75f;
                P.TakeDamage(damage);
            }
        }
    }
}
