using UnityEngine;
using System.Collections;
using TMPro;

public class BookShieldSkill : SkillBase
{
    public float shieldDuration = 5f;
    public float shieldRange = 10f;
    public GameObject shieldPrefab;
    public LayerMask playerLayer;
    public PlayerDataSO PlayerData;

    private void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        lastActionTime = Time.time;
        Transform closestPlayer = FindClosestPlayer(transform.position, shieldRange, playerLayer);

        if (closestPlayer != null)
        {
            StartCoroutine(ApplyShield(closestPlayer));
        }
        else
        {
            Debug.Log("범위 내 플레이어 없음.");
        }
    }

    private Transform FindClosestPlayer(Vector3 position, float range, LayerMask layer)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, range, layer);
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.transform == transform) continue;

            float distance = Vector3.Distance(position, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = hitCollider.transform;
            }
        }

        return closestPlayer;
    }

    private IEnumerator ApplyShield(Transform target)
    {
        GameObject shield = Instantiate(shieldPrefab, target.position, Quaternion.identity);
        shield.transform.SetParent(target);

        Debug.Log($"{target.name}에게 보호막 적용!");

        yield return new WaitForSeconds(shieldDuration);

        Destroy(shield);
        Debug.Log($"{target.name}의 보호막 종료!");
    }
}
