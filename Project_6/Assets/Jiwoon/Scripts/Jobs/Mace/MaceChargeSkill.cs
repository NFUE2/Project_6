using UnityEngine;
using System.Collections;

public class MaceChargeSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    public float dashDuration = 0.5f; // �뽬 ���� �ð�
    public float dashSpeed = 10f; // �뽬 �ӵ�
    public LayerMask bossLayer; // ���� ���̾�
    public float dashDamage = 20f; // �뽬 ������
    public float reducedDamageDuringDash = 0.5f; // �뽬 �� �޴� ������ ���� ����

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

        // ������ ���� ����
        float originalDamageReduction = damageReduction;
        damageReduction *= reducedDamageDuringDash;

        while (Time.time - startTime < dashDuration)
        {
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // �浹 ����
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    // ������ ������ ����
                    if (hitCollider.TryGetComponent<IDamagable>(out var enemy))
                    {
                        enemy.TakeDamage(dashDamage);
                    }

                    // �������� Ȯ��
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
                            forceDirection.y = 0; // ���� ������ �� ����
                            enemyRb.AddForce(forceDirection.normalized * 5f, ForceMode.Impulse);
                        }
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
    }
}
