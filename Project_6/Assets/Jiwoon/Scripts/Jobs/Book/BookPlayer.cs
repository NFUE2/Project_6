using System.Collections;
using UnityEngine;
using TMPro;

public class BookPlayer : RangedPlayerBase
{
    public PlayerDataSO PlayerData;

    [Header("Skill Q")]
    [SerializeField] private BookShieldSkill bookshieldSkill;

    [Header("Skill E")]
    [SerializeField] private LaserSkill laserSkill;

    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;
    private float attackRange;
    private LayerMask targetLayer;
    private Vector3 projectileSpeed;
    private GameObject projectilePrefab;

    private void Start()
    {
        bookshieldSkill.SetCooldownText(qCooldownText);
        laserSkill.SetCooldownText(eCooldownText);
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime >= attackTime)
        {
            Transform closestTarget = FindClosestTarget(transform.position, attackRange, targetLayer);

            if (closestTarget != null)
            {
                Debug.Log("타겟 공격!");
                LaunchProjectile(closestTarget);
                lastAttackTime = Time.time;
            }
            else
            {
                Debug.Log("범위 내 타겟 없음.");
            }
        }
    }

    public override void UseSkillQ()
    {
        bookshieldSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        laserSkill.UseSkill();
    }

    private Transform FindClosestTarget(Vector3 position, float range, LayerMask layer)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, range, layer);
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.transform == transform) continue;

            float distance = Vector3.Distance(position, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = hitCollider.transform;
            }
        }

        return closestTarget;
    }

    private void LaunchProjectile(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        Destroy(projectile, 5f);
    }
}
