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
    public int damage = 10; // 데미지 값 추가
    public AudioClip dashSound; // 대시 효과음 추가
    public GameObject hitEffectPrefab; // 파티클 효과 프리팹 추가
    private AudioSource audioSource;

    private void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        lastActionTime = Time.time;
        PlayDashSound();
        StartCoroutine(Dash());
    }

    private void PlayDashSound()
    {
        if (dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }
    }

    private IEnumerator Dash()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition;

        // 플레이어가 바라보는 방향으로 대시
        if (playerTransform.localScale.x > 0)
        {
            endPosition = transform.position + transform.right * -dashDistance;
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
        Vector2 boxCenter = (startPosition + endPosition) / 2;
        Vector2 boxSize = new Vector2(dashDistance, playerTransform.localScale.y);
        Vector2 direction = (endPosition - startPosition).normalized;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(startPosition, boxSize, 0f, direction, dashDistance, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out IPunDamagable m))
            {
                Debug.Log($"Dealing damage to: {hit.collider.name}");
                m.Damage(damage);

                // 파티클 효과 생성
                if (hitEffectPrefab != null)
                {
                    Vector3 effectPosition = hit.transform.position; // 적의 transform 위치를 기준으로 함
                    effectPosition.y += 0.5f; // 적의 위치보다 약간 위로 파티클 효과를 생성

                    Instantiate(hitEffectPrefab, effectPosition, Quaternion.identity);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = (transform.position + transform.position + transform.right * dashDistance) / 2;
        Vector2 boxSize = new Vector2(dashDistance, playerTransform.localScale.y); // 1f 대신 플레이어의 높이를 사용
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
