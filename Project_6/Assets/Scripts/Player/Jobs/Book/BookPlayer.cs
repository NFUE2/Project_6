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
    [SerializeField] private TextMeshProUGUI noTargetText; // ���� ����� ���� �� ǥ�õ� �ؽ�Ʈ
    private float messageDisplayDuration = 2f; // �޽����� ǥ�õ� �ð�(��)

    private void Start()
    {
        bookshieldSkill.SetCooldownImage(qCooldownImage);
        laserSkill.SetCooldownImage(eCooldownImage);

        // �ؽ�Ʈ �޽����� ó������ ��Ȱ��ȭ
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
            // ���� ����� ���� �� �ؽ�Ʈ �޽��� ǥ��
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
            noTargetText.gameObject.SetActive(true); // �ؽ�Ʈ Ȱ��ȭ
            noTargetText.text = "No Target in Range!"; // ǥ���� �޽���
            StartCoroutine(HideNoTargetMessageAfterDelay());
        }
    }

    private IEnumerator HideNoTargetMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDisplayDuration);
        if (noTargetText != null)
        {
            noTargetText.gameObject.SetActive(false); // ���� �ð� �� �ؽ�Ʈ ��Ȱ��ȭ
        }
    }
}

