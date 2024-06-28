using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public int maxHealth = 100; // 최대 체력
    private int currentHealth;
    private BoxCollider2D damagable;

    private void Awake()
    {
        GetComponent<BoxCollider2D>();
    }

    // 데미지를 받는 메서드
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("플레이어 체력: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 플레이어가 죽었을 때 호출되는 메서드
    private void Die()
    {
        Debug.Log("플레이어 사망");
        Destroy(gameObject); // 플레이어 오브젝트 파괴한다
    }
}