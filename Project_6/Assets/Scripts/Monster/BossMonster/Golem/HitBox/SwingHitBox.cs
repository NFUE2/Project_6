using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHitBox : HitBox
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.TryGetComponent<IDamagable>(out IDamagable P) && collision.TryGetComponent<IKnockBackable>(out IKnockBackable K))
            {
                float damage = BossBattleManager.Instance.boss.attackPower * 0.75f;
                P.TakeDamage(damage);
                Vector2 playerPos = collision.transform.position;
                Vector2 bossPos = BossBattleManager.Instance.spawnedBoss.transform.position;

                Vector2 knockbackDirection = bossPos.x < playerPos.x ? new Vector2(1, 0) : new Vector2(-1, 0);
                K.ApplyKnockback(knockbackDirection, 5);
            }
        }
    }
}
