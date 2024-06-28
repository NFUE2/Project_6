using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public int maxHealth = 100; // �ִ� ü��
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth; // �ʱ� ü���� �ִ� ü������ ����
    }

    // �������� �޴� �޼���
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("�÷��̾� ü��: " + currentHealth);

        if (currentHealth <= 0)
        {
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
