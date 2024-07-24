using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ChargeSkill : SkillBase
{
    public float maxChargingTime; // 최대 충전 시간
    public bool isCharging; // 충전 중인지 여부
    public float damage; // 기본 피해량
    public float damageRate; // 충전 시 증가하는 피해량 비율
    public PlayerDataSO PlayerData; // 플레이어 데이터
    public Vector2 attackSize; // 공격 박스 크기
    public Vector2 attackOffset; // 공격 박스 오프셋
    public LayerMask enemyLayer; // 적 레이어

    private bool isSkillAttack; // 스킬 공격 여부
    private Animator animator; // 애니메이터
    private float currentDamage; // 현재 피해량
    private bool isAttacking; // 공격 중인지 여부
    private Rigidbody2D rb; // Rigidbody2D 참조
    private CharacterController characterController; // CharacterController 참조 (있을 경우)
    private bool wasKinematic; // Rigidbody2D의 원래 kinematic 상태 저장

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        characterController = GetComponent<CharacterController>();
        if (rb != null) wasKinematic = rb.isKinematic;
        cooldownDuration = PlayerData.SkillECooldown;
        lastActionTime = -cooldownDuration; // 초기 쿨다운 설정
    }

    public override void UseSkill()
    {
        if (isCharging || Time.time - lastActionTime < cooldownDuration) return;

        isCharging = true;
        animator.SetBool("IsCharging", true);
        DisableMovement();
        StartCoroutine(Charging());
    }

    private void DisableMovement()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true; // Rigidbody2D가 있을 경우 비활성화
        }
        if (characterController != null)
        {
            characterController.enabled = false; // CharacterController가 있을 경우 비활성화
        }
    }

    private void EnableMovement()
    {
        if (rb != null)
        {
            rb.isKinematic = wasKinematic; // 원래 kinematic 상태 복원
        }
        if (characterController != null)
        {
            characterController.enabled = true; // CharacterController 다시 활성화
        }
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
        PerformAttack(); // 충전이 끝난 후에 공격 수행
        EnableMovement(); // 움직임 다시 활성화
    }

    private void PerformAttack()
    {
        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<IDamagable>()?.TakeDamage(currentDamage);
        }

        currentDamage = damage;
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
