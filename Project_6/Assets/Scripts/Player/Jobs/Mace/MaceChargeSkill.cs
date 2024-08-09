using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaceChargeSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    public float dashDuration = 0.5f; // �뽬 ���� �ð�
    public float dashSpeed = 10f; // �뽬 �ӵ�
    public LayerMask bossLayer; // ���� ���̾�
    public LayerMask targetLayer; // Ÿ�� ���̾�

    public float dashDamage = 20f; // �뽬 ������
    public float reducedDamageDuringDash = 0.5f; // �뽬 �� �޴� ������ ���� ����

    private bool isDashing;
    private bool bossHit;
    protected bool enhancedAttack;
    public float damageReduction;

    public AudioClip skillSound; // ��ų ȿ����
    public AudioClip hitSound; // ������ �ε����� �� ȿ����
    private AudioSource audioSource; // ����� �ҽ�

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
        PlaySkillSound(); // ��ų ��� �� ȿ���� ���
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

        // ������ ���� ����
        float originalDamageReduction = damageReduction;
        damageReduction *= reducedDamageDuringDash;

        // �뽬 ���� ����
        Vector2 dashDirection = new Vector2(transform.localScale.x * -1, 0); // �뽬 ���� ����

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
                    PlayHitSound(); // ������ �ε����� �� ȿ���� ���
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
                        enemyRb.AddForce(forceDirection * dashSpeed, ForceMode2D.Impulse); // �뽬 �ӵ��� ���� �б�
                        Debug.Log("�� �и�");
                    }
                }
            }

            yield return null;
        }

        // �뽬 ���� �� ���� ������ ���� ���� ����
        damageReduction = originalDamageReduction;
        isDashing = false;

        if (bossHit)
        {
            enhancedAttack = true; // ���� ���� ��ȭ
        }

        hitEnemies.Clear();
    }
}

