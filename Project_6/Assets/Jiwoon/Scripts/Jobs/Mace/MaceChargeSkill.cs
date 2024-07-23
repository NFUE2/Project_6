using UnityEngine;
using System.Collections;

public class MaceChargeSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    public float dashDuration = 0.5f; // 대쉬 지속 시간
    public float dashSpeed = 10f; // 대쉬 속도
    public LayerMask bossLayer; // 보스 레이어
    public float dashDamage = 20f; // 대쉬 데미지
    public float reducedDamageDuringDash = 0.5f; // 대쉬 중 받는 데미지 감소 비율

    private bool isDashing;
    private bool bossHit;
    protected bool enhancedAttack;
    public float damageReduction;

    private void Start()
    {
        if (PlayerData == null)
        {
            Debug.LogWarning("PlayerDataSO is not assigned!");
            return;
        }

        cooldownDuration = PlayerData.SkillECooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration || isDashing) return;

        lastActionTime = Time.time;
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        bossHit = false;
        float startTime = Time.time;

        // 데미지 감소 설정
        float originalDamageReduction = damageReduction;
        damageReduction *= reducedDamageDuringDash;

        while (Time.time - startTime < dashDuration)
        {
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // 충돌 범위
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    // 적에게 데미지 입힘
                    if (hitCollider.TryGetComponent<IDamagable>(out var enemy))
                    {
                        enemy.TakeDamage(dashDamage);
                    }

                    // 보스인지 확인
                    if (bossLayer == (bossLayer | (1 << hitCollider.gameObject.layer)))
                    {
                        bossHit = true;
                    }
                    else
                    {
                        // 적 밀어내기 및 스턴
                        Rigidbody enemyRb = hitCollider.GetComponent<Rigidbody>();
                        if (enemyRb != null)
                        {
                            Vector3 forceDirection = hitCollider.transform.position - transform.position;
                            forceDirection.y = 0; // 수직 방향의 힘 제거
                            enemyRb.AddForce(forceDirection.normalized * 5f, ForceMode.Impulse);
                        }
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
    }
}
