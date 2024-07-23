using UnityEngine;

public class Hammer_Shield : MonoBehaviour
{
    public float maxDamage; // 방어막이 받을 수 있는 최대 데미지
    public bool isBossAttack; // 보스 공격 여부를 체크하는 변수

    private float currentDamage; // 현재 받은 데미지

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

