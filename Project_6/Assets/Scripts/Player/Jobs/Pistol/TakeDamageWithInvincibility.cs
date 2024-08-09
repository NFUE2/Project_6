
using UnityEngine;

public class Rolling_Skill_Invincible : MonoBehaviour, IDamagable
{
    private PlayerCondition playerCondition;
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
