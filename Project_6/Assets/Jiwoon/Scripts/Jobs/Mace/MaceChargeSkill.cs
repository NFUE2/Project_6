using UnityEngine;
using System.Collections;
using Photon.Pun;

public class MaceChargeSkill : SkillBase
{
    public float dashSpeed = 10f; // 돌진 속도
    public float dashDuration = 0.5f; // 돌진 지속 시간
    public float dashDamage = 20f; // 돌진 피해량
    public float reducedDamage = 0.5f; // 돌진 중 받는 피해 감소 비율
    public LayerMask bossLayer; // 보스 레이어
    public PlayerDataSO PlayerData;

    private bool isDashing;
    private bool bossHit;
    protected bool enhancedAttack;

    private void Start()
    {
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

        while (Time.time - startTime < dashDuration)
        {
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // 충돌 범위
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    //hitCollider.GetComponent<Enemy>().TakeDamage(dashDamage);
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
                            forceDirection.y = 0;
                            enemyRb.AddForce(forceDirection.normalized * 5f, ForceMode.Impulse);
                        }
                    }
                }
            }

            yield return null;
        }

        isDashing = false;
        if (bossHit)
        {
            enhancedAttack = true; // 다음 공격 강화
        }
    }
}
