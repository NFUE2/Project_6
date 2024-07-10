using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class PistolPlayer : PlayerBase
{
    [Header("Attack")]
    private bool isAttackCooldown = false;
    private int attackCount = 0;
    private float cooldownDuration = 3f; //������ ���� ��Ÿ��

    // ���� ���� - ĳ���� ����κ��� PlayerBase�� �÷��ּ���
    private float attackTime;
    private float lastAttackTime;
    //=====================================================

    private Camera mainCamera;
    private bool isRolling;
    private bool fanningReady;
    public Transform attackPoint; //�߻���ġ
    public GameObject attackPrefab; //�߻��� ������Ʈ ����

    // ���� ���� - ĳ���� ����κ��� PlayerBase�� �÷��ּ���
    [Header("Skill Q")]
    public float actionTime;
    private float lastAction;
    //====================================================

    public bool fanningReadyQ;

    public override void Attack()
    {
        if (isAttackCooldown) return;
        if (fanningReady || isRolling) return;

        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        attackCount++;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // ���콺�� ��ġ��
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // ���콺�� ��ġ������ �������� ���� ���� ��ġ���� ���� => ���� ����
        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.
        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.

        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // ���� �ӵ� ���� �Ѵ�.

        if (attackCount >= 6)
        {
            StartCoroutine(AttackCooldown());
        }
    }
    public IEnumerator AttackCooldown() //6���� ���� ������ �Ѵ�.
    {
        isAttackCooldown = true;
        attackCount = 0;
        yield return new WaitForSeconds(cooldownDuration);
        isAttackCooldown = false;
    }

    public override void UseSkillQ()
    {
        //������ ��ũ��Ʈ �ʿ�
        if (fanningReady) return;
        if (Time.time - lastAction < actionTime) return;

        fanningReady = GetComponent<PlayerController_Gun>().fanningReady = true;
        //StartCoroutine(Fanning());
    }

    public override void UseSkillE()
    {
        // ������ E ��ų ����
    }
}
