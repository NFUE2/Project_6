using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaceChargeSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    public float dashDuration = 0.5f; // �뽬 ���� �ð�
    public float dashSpeed = 10f; // �뽬 �ӵ�
    public LayerMask bossLayer; // ���� ���̾�
    public LayerMask targetLayer; // ���� ���̾�

    public float dashDamage = 20f; // �뽬 ������
    public float reducedDamageDuringDash = 0.5f; // �뽬 �� �޴� ������ ���� ����

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

        // ������ ���� ����
        float originalDamageReduction = damageReduction;
        damageReduction *= reducedDamageDuringDash;


        while (Time.time - startTime < dashDuration)
        {
            //transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            transform.Translate(Vector2.right * (dashSpeed * Time.deltaTime * -transform.localScale.x));

            //Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // �浹 ����
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f,targetLayer); // �� ����

            foreach (var hitCollider in hitColliders)
            {
                if (list.Contains(hitCollider)) continue;
                //if (hitCollider.CompareTag("Enemy"))
                {
                    // ������ ������ ����
                    if (hitCollider.TryGetComponent<IDamagable>(out var enemy))
                    {
                        enemy.TakeDamage(dashDamage);
                        list.Add(hitCollider);
                    }

                    // �������� Ȯ��
                    //if (bossLayer == (bossLayer | (1 << hitCollider.gameObject.layer)))
                    if (hitCollider.CompareTag("Boss"))
                    {
                        bossHit = true;
                    }
                    else
                    {
                        // �� �о�� �� ����
                        Rigidbody2D enemyRb = hitCollider.GetComponent<Rigidbody2D>();
                        if (enemyRb != null)
                        {
                            Vector3 forceDirection = hitCollider.transform.position - transform.position;
                            forceDirection.y = 0; // ���� ������ �� ����
                            enemyRb.AddForce(forceDirection.normalized * 5f,ForceMode2D.Impulse);
                            Debug.Log("�и�");
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

        list.Clear();
    }
}
