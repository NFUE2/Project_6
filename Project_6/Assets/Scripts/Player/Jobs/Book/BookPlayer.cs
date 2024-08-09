using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Unity.VisualScripting;

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
            LaunchProjectile(closestTarget);
            lastAttackTime = Time.time;
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

        foreach (Collider2D hitCollider in hitColliders)
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
        Vector3 targetPosition = target.position;

        // 타겟의 콜라이더가 있는 경우 콜라이더의 오프셋을 추가
        Collider2D targetCollider = target.GetComponent<Collider2D>();
        if (targetCollider != null)
        {
            targetPosition = targetCollider.bounds.center;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;

        // Projectile 클래스의 SetDirection 메서드 호출
        //Projectile projectileComponent = projectile.GetComponent<Projectile>();
        //if (projectileComponent != null)
        //{
        //    projectileComponent.SetDirection(direction);
        //}
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        GameObject projectile = PhotonNetwork.Instantiate(attackPrefab.name, transform.position, Quaternion.Euler(0,0,angle));

        StartCoroutine(MoveProjectile(projectile, targetPosition));
    }

    private IEnumerator MoveProjectile(GameObject projectile, Vector3 targetPosition)
    {
        while (projectile != null)
        {
            Vector3 direction = (targetPosition - projectile.transform.position).normalized;
            projectile.transform.position += direction * projectileSpeed * Time.deltaTime;

            // 목표물과의 거리 체크
            if (Vector3.Distance(projectile.transform.position, targetPosition) < 0.1f)
            {
                // 충돌 시 처리 로직 추가 (예: 데미지 적용, 투사체 파괴 등)
                Destroy(projectile);
            }

            yield return null;
        }
    }
}
