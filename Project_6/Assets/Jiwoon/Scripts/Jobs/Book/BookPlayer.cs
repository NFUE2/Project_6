using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookPlayer_ : PlayerBase
{
    public PlayerData PlayerData;

    public TextMeshPro qCooldownText; // Q 스킬 쿨타임을 표시하는 UI 텍스트 요소
    public TextMeshPro eCooldownText; // E 스킬 쿨타임을 표시하는 UI 텍스트 요소

    [Header("Animation Data")]
    public Animator animator;

    [Header("Skill Q")]
    public float shieldDuration = 5f; // 보호막 지속 시간
    public float shieldRange = 10f; // 보호막 적용 범위
    public GameObject shieldPrefab; // 보호막 프리팹
    public LayerMask playerLayer; // 플레이어 레이어

    [Header("Skill E")]
    public float laserDuration = 2f; // 레이저 지속 시간
    public float laserDamage = 10f; // 레이저 데미지
    public LineRenderer laserRenderer; // 레이저를 그릴 LineRenderer
    public LayerMask enemyLayer; // 적 레이어

    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;
    public float attackRange = 10f; // 공격 범위
    public GameObject projectilePrefab; // 발사체 프리팹
    public LayerMask targetLayer; // 타겟 레이어
    public float projectileSpeed; //발사속도

    public override void Attack()
    {
        if (Time.time - lastAttackTime < attackTime) return; // 공격 딜레이 체크

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 클릭 위치에서 일정 범위 내 가장 가까운 타겟 찾기
        Transform closestTarget = FindClosestTarget(worldMousePosition, attackRange, targetLayer);

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

    Transform FindClosestTarget(Vector3 position, float range, LayerMask layer)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, range, layer);
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.transform == transform) continue; // 본인 제외

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
        // 발사체를 일정 시간 후 자동으로 삭제
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
            Debug.Log("범위 내 플레이어 없음.");
        }
    }

    private IEnumerator ApplyShield(Transform target)
    {
        // 보호막 오브젝트 생성 및 타겟에 적용
        GameObject shield = Instantiate(shieldPrefab, target.position, Quaternion.identity);
        shield.transform.SetParent(target); // 타겟의 자식으로 설정하여 따라다니도록 함

        Debug.Log($"{target.name}에게 보호막 적용!");

        // 보호막 지속 시간 동안 대기
        yield return new WaitForSeconds(shieldDuration);

        // 보호막 제거
        Destroy(shield);
        Debug.Log($"{target.name}의 보호막 종료!");
    }

    private IEnumerator CoolTimeQ()
    {
        lastQActionTime = Time.time;

        while (Time.time - lastQActionTime < PlayerData.SkillQCooldown)
        {
            float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastQActionTime);
            qCooldownText.text = $"Q스킬 남은 시간: {remainingTime:F1}초"; // 쿨타임 텍스트 갱신
            yield return null;
        }
        qCooldownText.text = "Q스킬 쿨타임 완료"; // 쿨타임 완료 텍스트 갱신
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
            laserRenderer.SetPosition(1, transform.position + direction * 100); // 레이저의 끝점을 먼 거리로 설정

            foreach (RaycastHit hit in hits)
            {
                //hit.collider.GetComponent<Enemy>().TakeDamage(laserDamage); // 적에게 데미지 적용
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
            eCooldownText.text = $"E스킬 남은 시간: {remainingTime:F1}초"; // 쿨타임 텍스트 갱신
            yield return null;
        }
        eCooldownText.text = "E스킬 쿨타임 완료"; // 쿨타임 완료 텍스트 갱신
    }
}