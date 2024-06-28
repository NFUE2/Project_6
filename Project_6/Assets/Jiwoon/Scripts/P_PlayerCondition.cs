using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public static PlayerCondition Instance { get; private set; } // �̱��� �ν��Ͻ�

    public int maxHealth = 100; // �ִ� ü��
    private int currentHealth;
    private BoxCollider2D damagable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ ������Ʈ�� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���ο� �ν��Ͻ��� �ı�
        }

        damagable = GetComponent<BoxCollider2D>();
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
