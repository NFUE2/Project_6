using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class BookPlayer : RangedPlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private BookShieldSkill bookshieldSkill;

    [Header("Skill E")]
    [SerializeField] private LaserSkill laserSkill;

    [Header("Attack")]
    public float attackRange;
    public LayerMask targetLayer;
    public float projectileSpeed;

    private void Start()
    {
        bookshieldSkill.SetCooldownText(qCooldownText);
        laserSkill.SetCooldownText(eCooldownText);
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < playerData.attackCooldown) return;

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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, range, layer);
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        Debug.Log($"찾은 타겟 수: {hitColliders.Length}");

        foreach (Collider2D hitCollider in hitColliders)
        {
            Debug.Log($"충돌한 객체: {hitCollider.gameObject.name}, 레이어: {hitCollider.gameObject.layer}");
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
        GameObject projectile = PhotonNetwork.Instantiate(attackPrefab.name, transform.position, Quaternion.identity);

        //Vector2 direction = (target.position - transform.position).normalized;
        //GameObject projectile = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        //projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        //Destroy(projectile, 5f);
    }
}
