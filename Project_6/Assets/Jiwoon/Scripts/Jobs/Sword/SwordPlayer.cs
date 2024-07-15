using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using TMPro;

public class SwordPlayer : PlayerBase
{
    public PlayerData PlayerData;
    public TextMeshProUGUI qCooldownText; // Q ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���
    public TextMeshProUGUI eCooldownText; // E ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���

    [Header("Animation Data")]
    public Animator animator; // ���� �ִϸ��̼� ���� �߰� => Sword�� ���� �ִϸ��̼� ��Ʈ�ѷ�


    //��ųŬ������ �̵� - ���� ��ųŬ�������� ó�� ���ϸ� �����ּ���
    [Header("Skill Q")]
    private bool isGuard;

    [Header("Skill E")]
    public GameObject projectile;  //Sword �÷��̾ ��� ������Ʈ��. ���� ���� �߰�
    //===================================

    //���ݺκ� - ����Ŭ������ �̵�
    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;
    //====================================

    //�ӽ� ���� ��ġ
    public float attackRange = 2.0f; // ���� ���� �߰�
    public int attackDamage = 10; // ���� ������ �߰�
    public LayerMask enemyLayer; // �� ���̾� �߰�


    //
    public override void Attack()
    {
        if (isGuard) return; // ���� ���¿����� ���� �Ұ�
        if (Time.time - lastAttackTime < attackTime) return; // ���� ������ üũ
        Debug.Log("�Ϲݰ���!");
        lastAttackTime = Time.time;
        //animator.SetTrigger("Attack");

        //�ӽ� �Ϲݰ���
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            enemy.GetComponent<IDamagable>().TakeDamage(attackDamage);
        }
        //
    }
    //�ӽð��ݺ��̴� ����
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    

    //��ųŬ�������� ����
    public override void UseSkillQ()
    {
        if (isGuard)
        {
            ExitGuard();
        }
        else
        {
            if (Time.time - lastQActionTime < PlayerData.SkillQCooldown) return; // Q ��ų ��Ÿ�� üũ

            Debug.Log("Q ��ų ���");
            isGuard = true;
            //animator.SetBool("Guard", true);
            Invoke("ExitGuardEvent", 1.0f); //��۽�ų��, ����ð� �����ͷ� ������
        }
    }

    private void ExitGuard()
    {
        Debug.Log("���� ����");
        isGuard = false;
        //animator.SetBool("Guard", false);
        StartCoroutine(CoolTimeQ());
    }

    private IEnumerator CoolTimeQ()
    {
        lastQActionTime = Time.time;

        while (Time.time - lastQActionTime < PlayerData.SkillQCooldown)
        {
            float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastQActionTime);
            qCooldownText.text = $"{remainingTime:F1}"; // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        qCooldownText.text = "Q��ų ��Ÿ�� �Ϸ�"; // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }
    private void ExitGuardEvent()
    {
        if (isGuard) ExitGuard();
    }

    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < PlayerData.SkillECooldown) return; // E ��ų ��Ÿ�� üũ
        Debug.Log("E ��ų ���");
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //���� projectile ���� �߰� �Ҷ� �ּ� Ǯ��
        //GameObject go = PhotonNetwork.Instantiate("Prototype/" + projectile.name, transform.position, Quaternion.identity);
        //go.transform.localEulerAngles = new Vector3(0, 0, angle);

        StartCoroutine(CoolTimeE());
    }

    private IEnumerator CoolTimeE()
    {
        lastEActionTime = Time.time;

        while (Time.time - lastEActionTime < PlayerData.SkillECooldown)
        {
            float remainingTime = PlayerData.SkillECooldown - (Time.time - lastEActionTime);
            eCooldownText.text = $"{remainingTime:F1}"; // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        eCooldownText.text = "E��ų ��Ÿ�� �Ϸ�"; // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }
    //==========================
}

