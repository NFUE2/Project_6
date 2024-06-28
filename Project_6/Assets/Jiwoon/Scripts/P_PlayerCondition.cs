using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour
{
    public float maxHealth = 100f; // �ִ� ü��
    private float currentHealth;
    public GameObject boss;
    public P_BossMonster bossMonster;
    public Image currentHpBar;

    private void Awake()
    {
        currentHealth = maxHealth; // �ʱ� ü���� �ִ� ü������ ����
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

    // �������� �޴� �޼���
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("�÷��̾� ü��: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHpBar.fillAmount = 0;    
            Die();
        }
    }

    // �÷��̾ �׾��� �� ȣ��Ǵ� �޼���
    private void Die()
    {
        Debug.Log("�÷��̾� ���");
        bossMonster.players.Remove(gameObject);
        Destroy(gameObject); // �÷��̾� ������Ʈ �ı��Ѵ�
    }

}
