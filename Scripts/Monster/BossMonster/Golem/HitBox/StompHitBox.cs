using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompHitBox : HitBox
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.TryGetComponent<IDamagable>(out IDamagable P))
            {
                float damage = BossBattleManager.Instance.boss.attackPower * 1f;
                P.TakeDamage(damage);
            }
        }
    }
}
