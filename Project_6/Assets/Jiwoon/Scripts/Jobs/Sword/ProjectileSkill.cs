using UnityEngine;
using TMPro;

public class ProjectileSkill : SkillBase
{
    public GameObject projectilePrefab;
    public Transform attackPoint;
    public PlayerDataSO PlayerData;

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GameObject projectileInstance = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().SetDirection(dir);

        lastActionTime = Time.time;
    }
}
