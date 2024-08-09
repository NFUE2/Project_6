using UnityEngine;

public class TakeDamageWithInvincibility : MonoBehaviour, IDamagable
{
    public PlayerCondition playerCondition;
    private RollingSkill rollingSkill;

    void Start()
    {
        playerCondition = GetComponent<PlayerCondition>();
        rollingSkill = GetComponent<RollingSkill>();
    }

    public void TakeDamage(float damage)
    {
        if (rollingSkill != null && rollingSkill.IsInvincible())
        {
            return;
        }

        playerCondition.TakeDamage(damage);
    }
}
