using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public static PlayerCondition Instance { get; private set; } // 싱글톤 인스턴스

    public int maxHealth = 100; // 최대 체력
    private int currentHealth;
    private BoxCollider2D damagable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 오브젝트가 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 새로운 인스턴스를 파괴
        }

        damagable = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
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
