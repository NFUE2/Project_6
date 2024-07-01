using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_BombArrow : MonoBehaviour
{
    public float speed = 1f;
    public float bombRange = 1f;
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
        //Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, bombRange);

        //foreach (Collider2D c in col)
        //{
        //    //데미지 주기
        //}

        //Destroy(gameObject);
    }
}
