using UnityEngine;

public class Hammer_Shield : MonoBehaviour
{
    public float maxDamage; // 방어막이 받을 수 있는 최대 데미지
    private float currentDamage; // 현재 받은 데미지

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
            Destroy(other.gameObject); // 투사체 파괴
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
