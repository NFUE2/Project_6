using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ChargeSkill : SkillBase
{
    public float maxChargingTime; // 최대 충전 시간
    public float damage; // 기본 피해량
    public float damageRate; // 충전 시 증가하는 피해량 비율
    public PlayerDataSO PlayerData; // 플레이어 데이터
    public Vector2 attackSize; // 공격 박스 크기
    public Vector2 attackOffset; // 공격 박스 오프셋
    public LayerMask enemyLayer; // 적 레이어

    public GameObject hammerImpactEffect; // 망치 충돌 파티클 효과 프리팹
    public AudioClip hammerImpactSound; // 망치 충돌 사운드
    private AudioSource audioSource; // 오디오 소스

    private bool isCharging; // 충전 중인지 여부
    private bool isAttacking; // 스킬 공격 여부
    private Animator animator; // 애니메이터
    private float currentDamage; // 현재 피해량
    private Rigidbody2D rb; // Rigidbody2D 참조
    private CharacterController characterController; // CharacterController 참조 (있을 경우)
    private bool wasKinematic; // Rigidbody2D의 원래 kinematic 상태 저장
    private float originalGravityScale; // Rigidbody2D의 원래 gravityScale 상태 저장

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
        lastActionTime = -cooldownDuration; // 초기 쿨다운 설정
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
        animator.SetTrigger("IsAttack"); // 공격 애니메이션 트리거
    }

    // 애니메이션 이벤트에서 호출될 메서드
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
            Destroy(effect, 1.0f); // 효과가 1초 후에 사라지도록 설정
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
            return basePosition + new Vector2(-attackOffset.x, attackOffset.y); // 왼쪽을 바라볼 때
        }
        else
        {
            return basePosition + attackOffset; // 오른쪽을 바라볼 때
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
