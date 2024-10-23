using UnityEngine;

public class Test_Enemy : MonoBehaviour, IDamagable
{
    public float health = 100;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name}에게 {damage}의 데미지를 입었습니다. 남은 체력: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 죽었습니다.");
        // 죽는 애니메이션 또는 로직 추가
        Destroy(gameObject);
    }
}
