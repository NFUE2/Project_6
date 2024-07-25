using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaceChargeSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    public float dashDuration = 0.5f; // 대쉬 지속 시간
    public float dashSpeed = 10f; // 대쉬 속도
    public LayerMask bossLayer; // 보스 레이어
    public LayerMask targetLayer; // 보스 레이어

    public float dashDamage = 20f; // 대쉬 데미지
    public float reducedDamageDuringDash = 0.5f; // 대쉬 중 받는 데미지 감소 비율

    private bool isDashing;
    private bool bossHit;
    protected bool enhancedAttack;
    public float damageReduction;

    List<Collider2D> list = new List<Collider2D>();

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
            //transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            transform.Translate(Vector2.right * (dashSpeed * Time.deltaTime * -transform.localScale.x));

            //Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // 충돌 범위
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f,targetLayer); // 힐 범위

            foreach (var hitCollider in hitColliders)
            {
                if (list.Contains(hitCollider)) continue;
                //if (hitCollider.CompareTag("Enemy"))
                {
                    // 적에게 데미지 입힘
                    if (hitCollider.TryGetComponent<IDamagable>(out var enemy))
                    {
                        enemy.TakeDamage(dashDamage);
                        list.Add(hitCollider);
                    }

                    // 보스인지 확인
                    //if (bossLayer == (bossLayer | (1 << hitCollider.gameObject.layer)))
                    if (hitCollider.CompareTag("Boss"))
                    {
                        bossHit = true;
                    }
                    else
                    {
                        // 적 밀어내기 및 스턴
                        Rigidbody2D enemyRb = hitCollider.GetComponent<Rigidbody2D>();
                        if (enemyRb != null)
                        {
                            Vector3 forceDirection = hitCollider.transform.position - transform.position;
                            forceDirection.y = 0; // 수직 방향의 힘 제거
                            enemyRb.AddForce(forceDirection.normalized * 5f,ForceMode2D.Impulse);
                            Debug.Log("밀림");
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

        list.Clear();
    }
}
