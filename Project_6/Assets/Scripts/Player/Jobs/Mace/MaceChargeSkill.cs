using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaceChargeSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    public float dashDuration = 0.5f; // 대쉬 지속 시간
    public float dashSpeed = 10f; // 대쉬 속도
    public LayerMask bossLayer; // 보스 레이어
    public LayerMask targetLayer; // 타겟 레이어

    public float dashDamage = 20f; // 대쉬 데미지
    public float reducedDamageDuringDash = 0.5f; // 대쉬 중 받는 데미지 감소 비율

    private bool isDashing;
    private bool bossHit;
    protected bool enhancedAttack;
    public float damageReduction;

    public AudioClip skillSound; // 스킬 효과음
    public AudioClip hitSound; // 적에게 부딪쳤을 때 효과음
    private AudioSource audioSource; // 오디오 소스

    private Rigidbody2D rb;
    List<Collider2D> hitEnemies = new List<Collider2D>();

    private void Start()
    {
        if (PlayerData == null)
        {
            Debug.LogWarning("PlayerDataSO is not assigned!");
            return;
        }

        cooldownDuration = PlayerData.SkillECooldown;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration || isDashing) return;

        lastActionTime = Time.time;
        PlaySkillSound(); // 스킬 사용 시 효과음 재생
        StartCoroutine(Dash());
    }

    private void PlaySkillSound()
    {
        if (skillSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(skillSound);
        }
    }

    private void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        bossHit = false;
        float startTime = Time.time;

        // 데미지 감소 설정
        float originalDamageReduction = damageReduction;
        damageReduction *= reducedDamageDuringDash;

        // 대쉬 방향 설정
        Vector2 dashDirection = new Vector2(transform.localScale.x * -1, 0); // 대쉬 방향 설정

        while (Time.time - startTime < dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f, targetLayer);

            foreach (var hitCollider in hitColliders)
            {
                if (hitEnemies.Contains(hitCollider)) continue;

                if (hitCollider.TryGetComponent(out IDamagable enemy))
                {
                    enemy.TakeDamage(dashDamage);
                    //enemy.TakeDamage(dashDamage);
                    PlayHitSound(); // 적에게 부딪쳤을 때 효과음 재생
                    hitEnemies.Add(hitCollider);
                }

                if (hitCollider.CompareTag("Boss"))
                {
                    bossHit = true;
                }
                else
                {
                    Rigidbody2D enemyRb = hitCollider.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        Vector2 forceDirection = (hitCollider.transform.position - transform.position).normalized;
                        enemyRb.AddForce(forceDirection * dashSpeed, ForceMode2D.Impulse); // 대쉬 속도에 맞춰 밀기
                        Debug.Log("적 밀림");
                    }
                }
            }

            yield return null;
        }

        // 대쉬 종료 시 원래 데미지 감소 설정 복원
        damageReduction = originalDamageReduction;
        isDashing = false;

        if (bossHit)
        {
            enhancedAttack = true; // 다음 공격 강화
        }

        hitEnemies.Clear();
    }
}

