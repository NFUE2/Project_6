using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaceChargeSkill : SkillBase
{
    [Header("Skill Settings")]
    public PlayerDataSO PlayerData;
    public float dashDuration = 0.5f; // 대쉬 지속 시간
    public float dashSpeed = 10f; // 대쉬 속도
    public float dashDamage = 20f; // 대쉬 데미지
    public float defenseBoost = 10f; // 대쉬 중 방어력 증가량

    [Header("Audio Settings")]
    public AudioClip skillSound; // 스킬 효과음
    public AudioClip hitSound; // 적에게 부딪쳤을 때 효과음

    [Header("Layers")]
    public LayerMask targetLayer; // 타겟 레이어

    private bool isDashing;
    private bool bossHit;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private List<Collider2D> hitEnemies = new List<Collider2D>();
    private float originalDefense;

    private void Start()
    {
        if (PlayerData == null)
        {
            Debug.LogWarning("PlayerData is not assigned.");
            return;
        }

        cooldownDuration = PlayerData.SkillECooldown;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration || isDashing) return;

        lastActionTime = Time.time;
        PlaySound(skillSound);
        StartCoroutine(Dash());
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        bossHit = false;
        float startTime = Time.time;

        // 기존 방어력 저장
        originalDefense = PlayerData.defence;

        // 대쉬 중 방어력 증가
        PlayerData.defence += defenseBoost;

        Vector2 dashDirection = new Vector2(transform.localScale.x * -1, 0);

        while (Time.time - startTime < dashDuration)
        {
            Vector2 newPosition = rb.position + dashDirection * dashSpeed * Time.deltaTime;
            rb.MovePosition(newPosition);

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f, targetLayer);

            foreach (var hitCollider in hitColliders)
            {
                if (hitEnemies.Contains(hitCollider)) continue;

                if (hitCollider.TryGetComponent(out IPunDamagable enemy))
                {
                    enemy.Damage(dashDamage);
                    PlaySound(hitSound);
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
                        enemyRb.AddForce(forceDirection * dashSpeed, ForceMode2D.Impulse);
                    }
                }
            }

            yield return null;
        }

        // 방어력을 원래 값으로 복구
        PlayerData.defence = originalDefense;
        isDashing = false;

        if (bossHit)
        {
            // 다음 공격 강화 로직
        }

        hitEnemies.Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            // 충돌 시 대쉬를 종료하고 방어력 복구
            PlayerData.defence = originalDefense; // 방어력을 원래 값으로 복구
            isDashing = false;
        }
    }
}
