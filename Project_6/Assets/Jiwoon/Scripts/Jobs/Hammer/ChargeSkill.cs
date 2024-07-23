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

    void Start()
    {
        animator = GetComponent<Animator>();
        cooldownDuration = PlayerData.SkillECooldown;
        lastActionTime = -cooldownDuration; // 초기 쿨다운 설정
    }

    public override void UseSkill()
    {
        if (isCharging || Time.time - lastActionTime < cooldownDuration) return;

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
        isSkillAttack = true;
        lastActionTime = Time.time;
        PerformAttack();
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
