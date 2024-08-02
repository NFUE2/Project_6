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
            Debug.Log("Player is invincible and takes no damage.");
            return;
        }

        playerCondition.TakeDamage(damage);
    }
}
