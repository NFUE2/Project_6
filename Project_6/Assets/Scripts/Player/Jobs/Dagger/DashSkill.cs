using UnityEngine;
using System.Collections;
using Photon.Pun;

public class DashSkill : SkillBase
{
    public float dashDistance = 5f;
    public float dashSpeed = 10f;
    public Transform playerTransform;
    public LayerMask enemyLayer;
    public PlayerDataSO PlayerData;
    public float damage = 10f; // 데미지 값 추가

    private void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        lastActionTime = Time.time;
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition;
        if (playerTransform.localScale.x == 1)
        {
            endPosition = transform.position + transform.right * -(dashDistance);
        }
        else
        {
            endPosition = transform.position + transform.right * dashDistance;
        }

        float startTime = Time.time;

        while (Time.time < startTime + (dashDistance / dashSpeed))
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (Time.time - startTime) * dashSpeed / dashDistance);
            yield return null;
        }

        transform.position = endPosition;
        DealDamageToEnemiesOnPath(startPosition, endPosition);
    }

    private void DealDamageToEnemiesOnPath(Vector3 startPosition, Vector3 endPosition)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, endPosition - startPosition, dashDistance, enemyLayer);
        foreach (var hit in hits)
        {
            IDamagable enemy = hit.collider.GetComponent<IDamagable>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"적 {hit.collider.name}에게 데미지 {damage}!");
            }
        }
    }
}
