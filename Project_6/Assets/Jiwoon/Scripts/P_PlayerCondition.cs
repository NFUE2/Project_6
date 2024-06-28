using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour
{
    public float maxHealth = 100f; // 최대 체력
    private float currentHealth;
    public GameObject boss;
    public P_BossMonster bossMonster;
    public Image currentHpBar;

    private void Awake()
    {
        currentHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
    }

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        currentHpBar = GameObject.Find("Current_HP").GetComponent<Image>();
    }

    private void Update()
    {
        currentHpBar.fillAmount = currentHealth / maxHealth;
    }

    // 데미지를 받는 메서드
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("플레이어 체력: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHpBar.fillAmount = 0;    
            Die();
        }
    }

    // 플레이어가 죽었을 때 호출되는 메서드
    private void Die()
    {
        Debug.Log("플레이어 사망");
        bossMonster.players.Remove(gameObject);
        Destroy(gameObject); // 플레이어 오브젝트 파괴한다
    }

}
