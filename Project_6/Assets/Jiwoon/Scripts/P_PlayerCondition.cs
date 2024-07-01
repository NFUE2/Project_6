using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class P_PlayerCondition : MonoBehaviour, P_IDamagable
{
    public float maxHealth = 100f; // 최대 체력
    public float currentHealth;
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
        //Debug.Log(currentHealth / maxHealth);
        //Debug.Log(currentHpBar.fillAmount);
    }

    // 데미지를 받는 메서드
    public void TakeDamage(float damage)
    {
        P_GunE gunEComponent = GetComponent<P_GunE>(); //P_GunE스킬쓰기 위해 가져온다.

        if (gunEComponent != null && gunEComponent.isInvincible) // P_GunE 스킬과 연동
        {
            return;
        }

        currentHealth -= damage;
        currentHpBar.fillAmount = currentHealth / maxHealth;
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
        //PhotonNetwork.Destroy(gameObject);
        bossMonster.players.Remove(gameObject);
        PhotonNetwork.Destroy(gameObject); // 플레이어 오브젝트 파괴한다
    }
}
