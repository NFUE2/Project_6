using UnityEngine;

public class Hammer_Shield : MonoBehaviour
{
    public float maxDamage; // ���� ���� �� �ִ� �ִ� ������
    private float currentDamage; // ���� ���� ������

    void Start()
    {
        currentDamage = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            float damage = 10f;
            TakeDamage(damage, false);
            Destroy(other.gameObject); // ����ü �ı�
        }
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
