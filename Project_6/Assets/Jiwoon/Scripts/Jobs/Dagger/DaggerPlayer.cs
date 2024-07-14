using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using TMPro;

public class DaggerPlayer : PlayerBase
{
    public PlayerData PlayerData;

    public TextMeshPro qCooldownText; // Q ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���
    public TextMeshPro eCooldownText; // E ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���

    [Header("Animation Data")]
    public Animator animator; // ���� �ִϸ��̼� ���� �߰� => Dagger�� ���� �ִϸ��̼� ��Ʈ�ѷ�

    //��ųŬ������ �̵� - ���� ��ųŬ�������� ó�� ���ϸ� �����ּ���
    [Header("Skill Q")]
    public float dashDistance = 5f; // �뽬 �Ÿ�
    public float dashSpeed = 10f; // �뽬 �ӵ�
    public LayerMask enemyLayer; // �� ���̾� ����ũ

    [Header("Skill E")]
    public int currentStack = 0; // ���� ���� ��
    public int maxStack = 10; // �ִ� ���� ��
    public int damagePerStack = 10; // ���� �� ������
    public TextMeshPro stackText; // ������ ǥ���ϴ� UI �ؽ�Ʈ ���
    //====================================

    //���ݺκ� - ����Ŭ������ �̵�
    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;
    //====================================

    //���� ĳ���� Ŭ�������� ���� - �ʿ��ϸ� �� ��ũ��Ʈ �ۼ�
    public override void Attack()
    {
        if (Time.time - lastAttackTime < attackTime) return; // ���� ������ üũ
        Debug.Log("�Ϲݰ���!");
        lastAttackTime = Time.time;
        //animator.SetTrigger("Attack");

        // ������ �����ߴٰ� ����
        IncreaseStack();
    }

    //��ųŬ�������� ����
    private void IncreaseStack() //E��ų�� �������� ����
    {
        currentStack++;
        if (currentStack > maxStack)
        {
            currentStack = maxStack;
        }
        stackText.text = $"����: {currentStack}"; // ���� UI ����
    }
    public override void UseSkillQ()
    {
        StartCoroutine(Dash());
        StartCoroutine(CoolTimeQ());
    }
    private IEnumerator Dash()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + transform.forward * dashDistance;
        float startTime = Time.time;

        while (Time.time < startTime + (dashDistance / dashSpeed))
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (Time.time - startTime) * dashSpeed / dashDistance);
            yield return null;
        }

        transform.position = endPosition;
        DealDamageToEnemiesOnPath(startPosition, endPosition);
    }

    private void DealDamageToEnemiesOnPath(Vector3 startPosition, Vector3 endPosition)
    {
        RaycastHit[] hits = Physics.RaycastAll(startPosition, endPosition - startPosition, dashDistance, enemyLayer);
        foreach (var hit in hits)
        {
            // ���⿡ ������ �������� ������ �ڵ带 �߰�
            Debug.Log($"�� {hit.collider.name}���� ������!");
        }
    }
    private IEnumerator CoolTimeQ()
    {
        lastQActionTime = Time.time;

        while (Time.time - lastQActionTime < PlayerData.SkillQCooldown)
        {
            float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastQActionTime);
            qCooldownText.text = $"Q��ų ���� �ð�: {remainingTime:F1}��"; // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        qCooldownText.text = "Q��ų ��Ÿ�� �Ϸ�"; // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }

    public override void UseSkillE()
    {
        if (currentStack > 0)
        {
            DealDamageWithStack();
            currentStack = 0; // ���� ����
            stackText.text = "����: 0"; // ���� UI ����
            StartCoroutine(CoolTimeE());
        }
        else
        {
            Debug.Log("������ �����մϴ�."); // ������ ������ �� �޽���
        }
    }
    private void DealDamageWithStack()
    {
        // ������ �������� ������ ����
        int totalDamage = currentStack * damagePerStack;
        Debug.Log($"���� {currentStack}���� ����Ͽ� {totalDamage}�� �������� �������ϴ�.");

        // �ִϸ��̼� Ʈ���� (Į ����ħ)
        // animator.SetTrigger("Slash");

        // ���⼭ ������ �������� ������ �ڵ带 �߰��ϼ���.
    }

    private IEnumerator CoolTimeE()
    {
        lastEActionTime = Time.time;

        while (Time.time - lastEActionTime < PlayerData.SkillECooldown)
        {
            float remainingTime = PlayerData.SkillECooldown - (Time.time - lastEActionTime);
            eCooldownText.text = $"E��ų ���� �ð�: {remainingTime:F1}��"; // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        eCooldownText.text = "E��ų ��Ÿ�� �Ϸ�"; // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }
    //================================
}

