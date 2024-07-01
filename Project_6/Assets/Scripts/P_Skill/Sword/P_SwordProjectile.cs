using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SwordProjectile : MonoBehaviour
{
    public float speed;
    public float damage;
    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out P_BossMonster boss))
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
