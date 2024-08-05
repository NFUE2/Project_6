using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ChargeSkill : SkillBase
{
    public float maxChargingTime; // �ִ� ���� �ð�
    public float damage; // �⺻ ���ط�
    public float damageRate; // ���� �� �����ϴ� ���ط� ����
    public PlayerDataSO PlayerData; // �÷��̾� ������
    public Vector2 attackSize; // ���� �ڽ� ũ��
    public Vector2 attackOffset; // ���� �ڽ� ������
    public LayerMask enemyLayer; // �� ���̾�

    public GameObject hammerImpactEffect; // ��ġ �浹 ��ƼŬ ȿ�� ������
    public AudioClip hammerImpactSound; // ��ġ �浹 ����
    private AudioSource audioSource; // ����� �ҽ�

    private bool isCharging; // ���� ������ ����
    private bool isAttacking; // ��ų ���� ����
    private Animator animator; // �ִϸ�����
    private float currentDamage; // ���� ���ط�
    private Rigidbody2D rb; // Rigidbody2D ����
    private CharacterController characterController; // CharacterController ���� (���� ���)
    private bool wasKinematic; // Rigidbody2D�� ���� kinematic ���� ����
    private float originalGravityScale; // Rigidbody2D�� ���� gravityScale ���� ����

    void Start()
    {
        InitializeComponents();
        InitializePlayerData();
    }

    private void InitializeComponents()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        if (rb != null)
        {
            wasKinematic = rb.isKinematic;
            originalGravityScale = rb.gravityScale;
        }
    }

    private void InitializePlayerData()
    {
        cooldownDuration = PlayerData.SkillECooldown;
        lastActionTime = -cooldownDuration; // �ʱ� ��ٿ� ����
    }

    public override void UseSkill()
    {
        if (isCharging || isAttacking || Time.time - lastActionTime < cooldownDuration) return;

        isCharging = true;
        animator.SetBool("IsCharging", true);
        StartCoroutine(Charging());
    }

    private IEnumerator Charging()
    {
        float startCharging = Time.time;
        currentDamage = damage;
        while (!Input.GetKeyUp(KeyCode.E) && !(Time.time - startCharging > maxChargingTime))
        {
            currentDamage += Time.deltaTime * damageRate;
            yield return null;
        }
        isCharging = false;
        animator.SetBool("IsCharging", false);
        lastActionTime = Time.time;
        isAttacking = true;
        animator.SetTrigger("IsAttack"); // ���� �ִϸ��̼� Ʈ����
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ��� �޼���
    public void PerformAttack()
    {
        if (!isCharging && isAttacking)
        {
            Vector2 attackPosition = CalculateAttackPosition();
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out MonsterCondition e))
                    e.Damage(currentDamage);

                //enemy.GetComponent<IDamagable>()?.TakeDamage(currentDamage);
            }

            SpawnImpactEffect(attackPosition);
            PlayImpactSound();

            currentDamage = damage;
            isAttacking = false;
        }
    }

    private void SpawnImpactEffect(Vector2 position)
    {
        if (hammerImpactEffect != null)
        {
            GameObject effect = Instantiate(hammerImpactEffect, position, Quaternion.identity);
            Destroy(effect, 1.0f); // ȿ���� 1�� �Ŀ� ��������� ����
        }
    }

    private void PlayImpactSound()
    {
        if (hammerImpactSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hammerImpactSound);
        }
    }

    private Vector2 CalculateAttackPosition()
    {
        Vector2 basePosition = (Vector2)transform.position;

        if (transform.localScale.x < 0)
        {
            return basePosition + new Vector2(-attackOffset.x, attackOffset.y); // ������ �ٶ� ��
        }
        else
        {
            return basePosition + attackOffset; // �������� �ٶ� ��
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
