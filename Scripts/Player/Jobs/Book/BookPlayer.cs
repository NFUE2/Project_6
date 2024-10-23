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

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI noTargetText; // 공격 대상이 없을 때 표시될 텍스트
    private float messageDisplayDuration = 2f; // 메시지가 표시될 시간(초)

    private void Start()
    {
        bookshieldSkill.SetCooldownImage(qCooldownImage);
        laserSkill.SetCooldownImage(eCooldownImage);

        // 텍스트 메시지를 처음에는 비활성화
        if (noTargetText != null)
        {
            noTargetText.gameObject.SetActive(false);
        }
    }

    public override void Attack()
    {
        if (currentAttackTime < playerData.attackTime) return;

        Transform closestTarget = FindClosestTarget(transform.position, attackRange, targetLayer);

        if (closestTarget != null)
        {
            LaunchProjectile(closestTarget);
            currentAttackTime = 0f;
        }
        else
        {
            // 공격 대상이 없을 때 텍스트 메시지 표시
            ShowNoTargetMessage();
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

        Collider2D targetCollider = target.GetComponent<Collider2D>();
        if (targetCollider != null)
        {
            targetPosition = targetCollider.bounds.center;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject projectile = PhotonNetwork.Instantiate(attackPrefab.name, transform.position, Quaternion.Euler(0, 0, angle));
    }

    private void ShowNoTargetMessage()
    {
        if (noTargetText != null)
        {
            noTargetText.gameObject.SetActive(true); // 텍스트 활성화
            noTargetText.text = "No Target in Range!"; // 표시할 메시지
            StartCoroutine(HideNoTargetMessageAfterDelay());
        }
    }

    private IEnumerator HideNoTargetMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDisplayDuration);
        if (noTargetText != null)
        {
            noTargetText.gameObject.SetActive(false); // 일정 시간 후 텍스트 비활성화
        }
    }
}

