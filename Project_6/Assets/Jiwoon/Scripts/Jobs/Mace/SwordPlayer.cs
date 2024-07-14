using Photon.Pun;
using UnityEngine;
using System.Collections;

//Ŭ���� �̸��̶� �����̸��� �ٸ�, �����ʿ�
public class MacePlayer : PlayerBase
{
    [Header("Animation Data")]
    public Animator animator; // ���� �ִϸ��̼� ���� �߰� => Sword�� ���� �ִϸ��̼� ��Ʈ�ѷ�

    //��ųŬ������ �̵� - ���� ��ųŬ�������� ó�� ���ϸ� �����ּ���
    [Header("Skill Q")]
    private bool isGuard;
    private float lastQActionTime;
    public float qSkillCooldown;
    public float healDuration = 5f; // �� ���� �ð�
    public float healAmount = 10f; // ����
    public float statBoostDuration = 10f; // ���� ��ȭ ���� �ð�
    public float defenseBoost = 10f; // ���� ������

    [Header("Skill E")]
    private bool isDashing;
    private float lastEActionTime;
    public float eSkillCooldown;
    public float dashSpeed = 10f; // ���� �ӵ�
    public float dashDuration = 0.5f; // ���� ���� �ð�
    public float dashDamage = 20f; // ���� ���ط�
    public float reducedDamage = 0.5f; // ���� �� �޴� ���� ���� ����
    public LayerMask bossLayer; // ���� ���̾�
    private bool bossHit; // ���� ��Ʈ ����
    //====================================

    [Header("Attack")]
    //���ݺκ� - ����Ŭ������ �̵�
    public float attackTime;
    private float lastAttackTime;
    //====================================

    private bool enhancedAttack;

    //PlayerDataSO���� ��������
    public float health = 100f; // �⺻ ü�� ��
    public float defense = 10f; // �⺻ ���� ��
    //=========================================


    //���� ĳ���� Ŭ�������� ����
    public override void Attack()
    {
        if (isGuard) return; // ���� ���¿����� ���� �Ұ�
        if (Time.time - lastAttackTime < attackTime) return; // ���� ������ üũ
        Debug.Log("�Ϲݰ���!");
        lastAttackTime = Time.time;

        float damage = 10f; // �⺻ ���� ������

        if (enhancedAttack)
        {
            damage += defense; // ���� �߰� ����
            enhancedAttack = false; // ��Ÿ ��ȭ ����
            Debug.Log($"��ȭ�� ����! �߰� ����: {defense}");
        }

        // ���� �ִϸ��̼� Ʈ����
        // animator.SetTrigger("Attack");

        // ������ ó�� ���� �߰� �ʿ�
    }

    //��ųŬ�������� ����
    public override void UseSkillQ()
    {
        if (Time.time - lastQActionTime < qSkillCooldown) return; // ��Ÿ�� üũ
        StartCoroutine(HealAndBoost());
    }

    private IEnumerator HealAndBoost()
    {
        lastQActionTime = Time.time;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // �� ����

        int healedPlayers = 0;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                MacePlayer player = hitCollider.GetComponent<MacePlayer>();
                if (player != null && player != this)
                {
                    StartCoroutine(HealPlayer(player));
                    healedPlayers++;
                }
            }
        }

        if (healedPlayers > 0)
        {
            StartCoroutine(BoostDefense(healedPlayers));
        }

        yield return new WaitForSeconds(qSkillCooldown);
        Debug.Log($"Q��ų ��Ÿ�� �Ϸ�"); // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }

    private IEnumerator HealPlayer(MacePlayer player)
    {
        float startTime = Time.time;
        while (Time.time - startTime < healDuration)
        {
            player.health += healAmount * Time.deltaTime;
            player.health = Mathf.Clamp(player.health, 0, 100); // ü�� ���� ����
            Debug.Log($"����: {healAmount * Time.deltaTime}, ���� ü��: {player.health}");
            yield return null;
        }
    }

    private IEnumerator BoostDefense(int healedPlayers)
    {
        float originalDefense = defense; // ���� ���� ����
        defense += defenseBoost * healedPlayers;
        yield return new WaitForSeconds(statBoostDuration);
        defense = originalDefense; // ���� ���󺹱�
    }

    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < eSkillCooldown || isDashing) return; // ��Ÿ�� üũ �� �ߺ� ���� ����
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        lastEActionTime = Time.time;
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

        yield return new WaitForSeconds(eSkillCooldown);
        Debug.Log($"E��ų ��Ÿ�� �Ϸ�"); // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }

    //==================================================
}
