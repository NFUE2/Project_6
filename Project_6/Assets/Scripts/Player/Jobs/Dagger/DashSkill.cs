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
        // 박스의 중심을 대시 시작과 끝의 중간 지점으로 설정
        Vector2 boxCenter = (startPosition + endPosition) / 2;
        // 박스의 크기를 대시 거리만큼 길게 설정하고 높이(또는 너비)를 넉넉하게 설정
        Vector2 boxSize = new Vector2(dashDistance, 1f); // 1f는 높이(또는 너비), 필요에 따라 조정 가능
        Vector2 direction = endPosition - startPosition;

        // BoxCastAll로 적을 감지
        RaycastHit2D[] hits = Physics2D.BoxCastAll(boxCenter, boxSize, 0f, direction, dashDistance, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out IPunDamagable m))
            {
                Debug.Log($"Dealing damage to: {hit.collider.name}");
                m.Damage(damage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = (transform.position + transform.position + transform.right * dashDistance) / 2;
        Vector2 boxSize = new Vector2(dashDistance, 1f); // 1f는 높이(또는 너비)
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }


}

