using UnityEngine;

public class Hammer_Shield : MonoBehaviour
{
    public float maxDamage; // ���� ���� �� �ִ� �ִ� ������
    public bool isBossAttack; // ���� ���� ���θ� üũ�ϴ� ����

    private float currentDamage; // ���� ���� ������

    void Start()
    {
        currentDamage = 0f;
    }

    public void TakeDamage(float damage, bool isBossAttack)
    {
        if (isBossAttack)
        {
            DestroyShield();
        }
        else
        {
            currentDamage += damage;
            if (currentDamage >= maxDamage)
            {
                DestroyShield();
            }
        }
    }

    public void DestroyShield()
    {
        Destroy(gameObject);
    }
}

