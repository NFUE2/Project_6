using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IDamagable>(out IDamagable P))
        {
            float damage = BossBattleManager.Instance.boss.attackPower * 0.75f;
            P.TakeDamage(damage);
        }
    }
}
