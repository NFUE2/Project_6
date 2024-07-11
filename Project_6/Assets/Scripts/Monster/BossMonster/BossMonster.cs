using UnityEngine;

public class BossMonster : MonoBehaviour, IDamagable
{
    public float maxHp { get; set; }
    public float attackPower { get; set; }
    public float defensePower { get; set; }
    public float moveSpeed {  get; set; }
    
    
    private float currentHp;

    public float GetFillAmountHP()
    {
        return (currentHp / maxHp);
    }

    public void TakeDamage(float damage)
    {
        currentHp -= (damage * (defensePower / 100));
        if (currentHp <= 0)
        {
            // ��� ó��
        }
    }
}