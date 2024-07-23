using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ChargeSkill : SkillBase
{
    public float maxChargingTime; // �ִ� ���� �ð�
    public bool isCharging; // ���� ������ ����
    public float damage; // �⺻ ���ط�
    public float damageRate; // ���� �� �����ϴ� ���ط� ����
    public PlayerDataSO PlayerData; // �÷��̾� ������
    public Vector2 attackSize; // ���� �ڽ� ũ��
    public Vector2 attackOffset; // ���� �ڽ� ������
    public LayerMask enemyLayer; // �� ���̾�

    private bool isSkillAttack; // ��ų ���� ����
    private Animator animator; // �ִϸ�����
    private float currentDamage; // ���� ���ط�
    private bool isAttacking; // ���� ������ ����

    void Start()
    {
        animator = GetComponent<Animator>();
        cooldownDuration = PlayerData.SkillECooldown;
        lastActionTime = -cooldownDuration; // �ʱ� ��ٿ� ����
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
            return basePosition + new Vector2(-attackOffset.x, attackOffset.y); // ������ �ٶ� ��
        }
        else
        {
            return basePosition + attackOffset; // �������� �ٶ� ��
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
