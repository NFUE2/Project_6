using UnityEngine;

public class Hammer_Shield : MonoBehaviour,IDamagable
{
    public float maxDamage; // ���� ���� �� �ִ� �ִ� ������
    private float currentDamage; // ���� ���� ������

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
    //        destroy(other.gameobject); // ����ü �ı�
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
