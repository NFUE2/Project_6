using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour
{
    public float maxHealth = 100f; // �ִ� ü��
    private float currentHealth;
    //public Image currentHpBar;

    private void Awake()
    {
        currentHealth = maxHealth; // �ʱ� ü���� �ִ� ü������ ����
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        //currentHpBar.fillAmount = currentHealth / maxHealth;
    }

    // �������� �޴� �޼���
    public void TakeDamage()
    {
        currentHealth -= 10f;
        Debug.Log("�÷��̾� ü��: " + currentHealth);

        if (currentHealth <= 0)
        {
            //currentHpBar.fillAmount = 0;    
            Die();
        }
    }

    // �÷��̾ �׾��� �� ȣ��Ǵ� �޼���
    private void Die()
    {
        Debug.Log("�÷��̾� ���");
        Destroy(gameObject); // �÷��̾� ������Ʈ �ı��Ѵ�
    }

}
