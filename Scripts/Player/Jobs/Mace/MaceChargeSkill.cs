using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaceChargeSkill : SkillBase
{
    [Header("Skill Settings")]
    public PlayerDataSO PlayerData;
    public float dashDuration = 0.5f; // �뽬 ���� �ð�
    public float dashSpeed = 10f; // �뽬 �ӵ�
    public float dashDamage = 20f; // �뽬 ������
    public float defenseBoost = 10f; // �뽬 �� ���� ������

    [Header("Audio Settings")]
    public AudioClip skillSound; // ��ų ȿ����
    public AudioClip hitSound; // ������ �ε����� �� ȿ����

    [Header("Layers")]
    public LayerMask targetLayer; // Ÿ�� ���̾�

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

        // ���� ���� ����
        originalDefense = PlayerData.defence;

        // �뽬 �� ���� ����
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

        // ������ ���� ������ ����
        PlayerData.defence = originalDefense;
        isDashing = false;

        if (bossHit)
        {
            // ���� ���� ��ȭ ����
        }

        hitEnemies.Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            // �浹 �� �뽬�� �����ϰ� ���� ����
            PlayerData.defence = originalDefense; // ������ ���� ������ ����
            isDashing = false;
        }
    }
}
