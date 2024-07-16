using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class BowPlayer : PlayerBase
{
    [Header("Attack")]
    private bool isAttackCooldown = false; 
    private int attackCount = 0;

    //���ݺκ� - ����Ŭ������ �̵�
    private float attackTime;
    private float lastAttackTime;
    //====================================

    
    private Camera mainCamera;

    public GameObject attackPrefab; //�Ѿ�
    public Transform attackPoint;
    private float cooldownDuration = 0.5f;

    //��ųŬ������ �̵� - ���� ��ųŬ�������� ó�� ���ϸ� �����ּ���
    [Header("Skill Q")]
    public GameObject wireArrow;

    [Header("Skill E")]
    public GameObject bombArrow;
    public float fireAngle;
    //====================================

    //���Ÿ� ĳ���Ϳ��� ����
    public override void Attack()
    {
        if (isAttackCooldown) return;

        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        attackCount++;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // ���콺�� ��ġ��
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // ���콺�� ��ġ������ �������� ���� ���� ��ġ���� ���� => ���� ����
        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.
        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.

        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // ���� �ӵ� ���� �Ѵ�.

        //lastAttackTime�� �ǵ�����,����
        if (attackCount >= 1)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    //����
    private IEnumerator AttackCooldown() //�߻�ӵ�
    {
        isAttackCooldown = true;
        attackCount = 0;
        yield return new WaitForSeconds(cooldownDuration);
        isAttackCooldown = false;
    }


    //��ųŬ�������� ����
    public override void UseSkillQ()
    {
        if (Time.time - lastQActionTime < qSkillCooldown) return;

        //GameObject go = Instantiate(wireArrow,/*hand.GetChild(0).*/transform.position,Quaternion.identity);
        GameObject go = PhotonNetwork.Instantiate("Prototype/" + name,/*hand.GetChild(0)*/transform.position, Quaternion.identity);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        go.transform.localEulerAngles = new Vector3(0, 0, angle);
        go.GetComponent<P_WireArrow>().player = transform;

        StartCoroutine(CoolTimeQ());
    }
    IEnumerator CoolTimeQ()
    {
        lastEActionTime = Time.time;

        while (Time.time - lastQActionTime < qSkillCooldown)
        {
            Debug.Log($"E��ų ���� �ð� : {lastEActionTime}"); // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        Debug.Log($"E��ų ��Ÿ�� �Ϸ�"); // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }

    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < eSkillCooldown) return;

        //float startAngle = hand.localEulerAngles.z - fireAngle;

        //Debug.Log(hand.localEulerAngles);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - fireAngle;

        for (int i = 0; i < 3; i++)
        {
            //GameObject go = Instantiate(bombArrow,/*hand.GetChild(0)*/transform.position,Quaternion.identity);
            GameObject go = PhotonNetwork.Instantiate("Prototype/" + bombArrow.name,/*hand.GetChild(0)*/transform.position, Quaternion.identity);

            go.transform.localEulerAngles = new Vector3(0, 0, angle + i * fireAngle);
        }
        StartCoroutine(CoolTimeE());
    }
    IEnumerator CoolTimeE()
    {
        lastEActionTime = Time.time;

        while (Time.time - lastEActionTime < eSkillCooldown)
        {
            Debug.Log($"E��ų ���� �ð� : {lastEActionTime}"); // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        Debug.Log($"E��ų ��Ÿ�� �Ϸ�"); // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }
    //========================================
}