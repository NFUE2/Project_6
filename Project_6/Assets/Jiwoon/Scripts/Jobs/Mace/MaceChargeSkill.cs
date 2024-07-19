using UnityEngine;
using System.Collections;
using Photon.Pun;

public class MaceChargeSkill : SkillBase
{
    public float dashSpeed = 10f; // ���� �ӵ�
    public float dashDuration = 0.5f; // ���� ���� �ð�
    public float dashDamage = 20f; // ���� ���ط�
    public float reducedDamage = 0.5f; // ���� �� �޴� ���� ���� ����
    public LayerMask bossLayer; // ���� ���̾�
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

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // �浹 ����
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
                        // �� �о�� �� ����
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
            enhancedAttack = true; // ���� ���� ��ȭ
        }
    }
}
