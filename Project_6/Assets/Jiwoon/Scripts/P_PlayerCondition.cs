using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class P_PlayerCondition : MonoBehaviour, P_IDamagable
{
    public float maxHealth = 100f; // �ִ� ü��
    public float currentHealth;
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
        //Debug.Log(currentHealth / maxHealth);
        //Debug.Log(currentHpBar.fillAmount);
    }

    // �������� �޴� �޼���
    public void TakeDamage(float damage)
    {
        P_GunE gunEComponent = GetComponent<P_GunE>(); //P_GunE��ų���� ���� �����´�.

        if (gunEComponent != null && gunEComponent.isInvincible) // P_GunE ��ų�� ����
        {
            return;
        }

        currentHealth -= damage;
        currentHpBar.fillAmount = currentHealth / maxHealth;
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
        //PhotonNetwork.Destroy(gameObject);
        bossMonster.players.Remove(gameObject);
        PhotonNetwork.Destroy(gameObject); // �÷��̾� ������Ʈ �ı��Ѵ�
    }
}
