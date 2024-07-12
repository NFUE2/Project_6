using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookPlayer_ : PlayerBase
{
    public PlayerData PlayerData;

    public TextMeshPro qCooldownText; // Q ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���
    public TextMeshPro eCooldownText; // E ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���

    [Header("Animation Data")]
    public Animator animator;

    [Header("Skill Q")]
    public float shieldDuration = 5f; // ��ȣ�� ���� �ð�
    public float shieldRange = 10f; // ��ȣ�� ���� ����
    public GameObject shieldPrefab; // ��ȣ�� ������
    public LayerMask playerLayer; // �÷��̾� ���̾�

    [Header("Skill E")]
    public float laserDuration = 2f; // ������ ���� �ð�
    public float laserDamage = 10f; // ������ ������
    public LineRenderer laserRenderer; // �������� �׸� LineRenderer
    public LayerMask enemyLayer; // �� ���̾�

    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;
    public float attackRange = 10f; // ���� ����
    public GameObject projectilePrefab; // �߻�ü ������
    public LayerMask targetLayer; // Ÿ�� ���̾�
    public float projectileSpeed; //�߻�ӵ�

    public override void Attack()
    {
        if (Time.time - lastAttackTime < attackTime) return; // ���� ������ üũ

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Ŭ�� ��ġ���� ���� ���� �� ���� ����� Ÿ�� ã��
        Transform closestTarget = FindClosestTarget(worldMousePosition, attackRange, targetLayer);

        if (closestTarget != null)
        {
            Debug.Log("Ÿ�� ����!");
            LaunchProjectile(closestTarget);
            lastAttackTime = Time.time;
        }
        else
        {
            Debug.Log("���� �� Ÿ�� ����.");
        }
    }

    Transform FindClosestTarget(Vector3 position, float range, LayerMask layer)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, range, layer);
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.transform == transform) continue; // ���� ����

            float distance = Vector3.Distance(position, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = hitCollider.transform;
            }
        }

        return closestTarget;
    }

    void LaunchProjectile(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        // �߻�ü�� ���� �ð� �� �ڵ����� ����
        Destroy(projectile, 5f);
    }

    public override void UseSkillQ()
    {
        Transform closestPlayer = FindClosestTarget(transform.position, shieldRange, playerLayer);

        if (closestPlayer != null)
        {
            StartCoroutine(ApplyShield(closestPlayer));
            StartCoroutine(CoolTimeQ());
        }
        else
        {
            Debug.Log("���� �� �÷��̾� ����.");
        }
    }

    private IEnumerator ApplyShield(Transform target)
    {
        // ��ȣ�� ������Ʈ ���� �� Ÿ�ٿ� ����
        GameObject shield = Instantiate(shieldPrefab, target.position, Quaternion.identity);
        shield.transform.SetParent(target); // Ÿ���� �ڽ����� �����Ͽ� ����ٴϵ��� ��

        Debug.Log($"{target.name}���� ��ȣ�� ����!");

        // ��ȣ�� ���� �ð� ���� ���
        yield return new WaitForSeconds(shieldDuration);

        // ��ȣ�� ����
        Destroy(shield);
        Debug.Log($"{target.name}�� ��ȣ�� ����!");
    }

    private IEnumerator CoolTimeQ()
    {
        lastQActionTime = Time.time;

        while (Time.time - lastQActionTime < PlayerData.SkillQCooldown)
        {
            float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastQActionTime);
            qCooldownText.text = $"Q��ų ���� �ð�: {remainingTime:F1}��"; // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        qCooldownText.text = "Q��ų ��Ÿ�� �Ϸ�"; // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }

    public override void UseSkillE()
    {
        StartCoroutine(FireLaser());
        StartCoroutine(CoolTimeE());
    }

    private IEnumerator FireLaser()
    {
        float startTime = Time.time;
        laserRenderer.enabled = true;

        while (Time.time - startTime < laserDuration)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 direction = (worldMousePosition - transform.position).normalized;

            Ray ray = new Ray(transform.position, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, enemyLayer);

            laserRenderer.SetPosition(0, transform.position);
            laserRenderer.SetPosition(1, transform.position + direction * 100); // �������� ������ �� �Ÿ��� ����

            foreach (RaycastHit hit in hits)
            {
                //hit.collider.GetComponent<Enemy>().TakeDamage(laserDamage); // ������ ������ ����
            }
            yield return null;
        }
        laserRenderer.enabled = false;
    }

    private IEnumerator CoolTimeE()
    {
        lastEActionTime = Time.time;

        while (Time.time - lastEActionTime < PlayerData.SkillECooldown)
        {
            float remainingTime = PlayerData.SkillECooldown - (Time.time - lastEActionTime);
            eCooldownText.text = $"E��ų ���� �ð�: {remainingTime:F1}��"; // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        eCooldownText.text = "E��ų ��Ÿ�� �Ϸ�"; // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }
}