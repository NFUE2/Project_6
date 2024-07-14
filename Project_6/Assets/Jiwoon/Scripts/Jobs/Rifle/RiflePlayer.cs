using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class RiflePlayer : PlayerBase
{
    public PlayerData PlayerData;

    [Header("Attack")]
    private bool isAttackCooldown = false;
    private int attackCount = 0;

    //���ݺκ� - ����Ŭ������ �̵�
    private float attackTime;
    private float lastAttackTime;
    //====================================

    private Camera mainCamera;

    //���Ÿ� ĳ���� Ŭ�������� ����
    public GameObject attackPrefab; // ����ü
    public Transform attackPoint;
    //======================================

    private float cooldownDuration = 0.5f; //? lastAttackTime�� �ٸ��� ���� �𸣰ھ��

    //��ųŬ������ �̵� - ���� ��ųŬ�������� ó�� ���ϸ� �����ּ���
    [Header("Skill Q")]
    public GameObject targetPrefab;
    public int qSkillMaxTargets = 3;
    public LayerMask enemyLayerMask;
    private List<GameObject> targetMarkers = new List<GameObject>();
    private int remainingChances = 3; // ���� ��ȸ ��

    [Header("Skill E")]
    public GameObject rifleGrenade;
    //====================================

    private void Start()
    {
        mainCamera = Camera.main;
    }

    //���Ÿ� ĳ���Ϳ��� ����
    public override void Attack()
    {
        if (isAttackCooldown) return;

        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        attackCount++;

        //�̰� input���� �� �� �ֵ��� ���ּ���, �̺�Ʈ ����� ����ϸ�˴ϴ�
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // ���콺�� ��ġ��


        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // ���콺�� ��ġ������ �������� ���� ���� ��ġ���� ���� => ���� ����
        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.
        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.

        //����ü Ŭ�������� ����
        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // ���� �ӵ� ���� �Ѵ�.


        //��Ʈ�׼��̶� 1�϶� �۵��ϴ� ��Ÿ���ε�,lastAttackTime�� �ǵ��� ������ �����ϴ�, ����
        if (attackCount >= 1) 
        {
            StartCoroutine(AttackCooldown());
        }
        //=========================
    }
    
    //����
    private IEnumerator AttackCooldown() //�߻�ӵ�
    {
        isAttackCooldown = true;
        attackCount = 0;
        yield return new WaitForSeconds(cooldownDuration);
        isAttackCooldown = false;
    }
    //===============================


    //��ųŬ�������� ����
    public override void UseSkillQ()
    {
        if (remainingChances <= 0) return;

        remainingChances--;

        // ���� ����� 3���� ���� ã��
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 10f, enemyLayerMask);
        var sortedEnemies = hitEnemies.OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).Take(qSkillMaxTargets);

        foreach (var enemy in sortedEnemies)
        {
            int markerCount = enemy.CompareTag("Boss") ? 3 : 1; // ������ 3���� ������, �Ϲ� ���� 1���� ������
            for (int i = 0; i < markerCount; i++)
            {
                Vector2 markerPosition = enemy.CompareTag("Boss") ? GetRandomPointInCollider(enemy) : enemy.transform.position;
                GameObject targetMarker = Instantiate(targetPrefab, markerPosition, Quaternion.identity);
                targetMarker.GetComponent<TargetMarker>().Initialize(enemy.gameObject, this);
                targetMarkers.Add(targetMarker);
            }
        }
    }

    private Vector2 GetRandomPointInCollider(Collider2D collider)
    {
        Bounds bounds = collider.bounds;
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }

    public void TargetMarkerClicked(GameObject enemy)
    {
        // ������ �������� ������ ���� �߰�
        // enemy.GetComponent<Enemy>().TakeDamage(10);
        Debug.Log($"������ ����: {enemy.name}");
        RemoveMarker();
    }

    public void TargetMarkerMissed()
    {
        Debug.Log("Ÿ�� ��Ŀ ��ħ");
        RemoveMarker();
    }


    private void RemoveMarker()
    {
        // ������ �ϳ� ���� �� ���� ��ȸ ������Ʈ
        remainingChances--;
        Debug.Log($"���� ��ȸ: {remainingChances}");
        if (remainingChances <= 0)
        {
            ClearAllMarkers();
        }
    }

    private void ClearAllMarkers()
    {
        foreach (var marker in targetMarkers)
        {
            if (marker != null)
            {
                Destroy(marker);
                Debug.Log("��Ŀ ����");
            }
        }
        targetMarkers.Clear();
        Debug.Log("��� ��Ŀ ���ŵ�");
    }
    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < PlayerData.SkillECooldown) return;

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        GameObject grenadeInstance = Instantiate(rifleGrenade, attackPoint.position, Quaternion.identity);
        grenadeInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 10f; // ����ü �ӵ� ����

        grenadeInstance.GetComponent<Grenade>().Initialize(10f, 5f, 3f); // ������, ����, ��Ʈ ���� �ð� �ʱ�ȭ

        lastEActionTime = Time.time;
        StartCoroutine(CoolTimeE());
    }

    private IEnumerator CoolTimeE()
    {
        lastEActionTime = Time.time;

        while (Time.time - lastEActionTime < PlayerData.SkillECooldown)
        {
            Debug.Log($"E��ų ���� �ð� : {lastEActionTime}"); // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        Debug.Log($"E��ų ��Ÿ�� �Ϸ�"); // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }

    //================================
}