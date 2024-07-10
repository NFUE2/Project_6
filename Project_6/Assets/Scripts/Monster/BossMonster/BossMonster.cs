using UnityEngine;

public class BossMonster : MonoBehaviour, IDamagable
{
    public float maxHp { get; set; }

    private float currentHp;
    public float attackPower { get; set; }
    public float defensePower { get; set; }
    public float moveSpeed {  get; set; }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            // »ç¸Á Ã³¸®
        }
    }
}