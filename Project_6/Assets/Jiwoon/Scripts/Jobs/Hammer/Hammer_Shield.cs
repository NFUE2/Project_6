using UnityEngine;

public class Hammer_Shield : MonoBehaviour,IDamagable
{
    public float maxDamage; // 방어막이 받을 수 있는 최대 데미지
    private float currentDamage; // 현재 받은 데미지

    void Start()
    {
        currentDamage = 0f;
    }
    //private void ontriggerenter2d(collider2d other)
    //{
    //    if (other.comparetag("enemyattack"))
    //    {
    //        float damage = 10f;
    //        takedamage(damage, false);
    //        destroy(other.gameobject); // 투사체 파괴
    //    }
    //}

    public void TakeDamage(float damage)
    {
        //if (isBossAttack)
        //{
        //    DestroyShield();
        //}
        //else
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
