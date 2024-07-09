using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;

public class SwordPlayer : PlayerBase
{

    [Header("Animation Data")]
    public Animator animator; // ���� �ִϸ��̼� ���� �߰� => Sword�� ���� �ִϸ��̼� ��Ʈ�ѷ�

    [Header("Skill Q")]
    private bool isGuard;
    private float qSkillCooldown;
    private float lastQActionTime;

    [Header("Skill E")]
    public GameObject projectile;  //Sword �÷��̾ ��� ������Ʈ��. ���� ���� �߰�
    private float eSkillCooldown;
    private float lastEActionTime;

    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;

    public override void Attack()
    {
        if (isGuard) return; // ���� ���¿����� ���� �Ұ�
        if (Time.time - lastAttackTime < attackTime) return; // ���� ������ üũ
        Debug.Log("�Ϲݰ���!");
        lastAttackTime = Time.time;
        //animator.SetTrigger("Attack");
    }

    public override void UseSkillQ()
    {
        if (isGuard)
        {
            ExitGuard();
        }
        else
        {
            if (Time.time - lastQActionTime < qSkillCooldown) return; // Q ��ų ��Ÿ�� üũ

            Debug.Log("Q ��ų ���");
            isGuard = true;
            //animator.SetBool("Guard", true);
            Invoke("ExitGuardEvent", 1.0f);
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

        while (Time.time - lastQActionTime < qSkillCooldown)
        {
            Debug.Log($"Q��ų ���� �ð� : {lastQActionTime}"); // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        Debug.Log($"Q��ų ��Ÿ�� �Ϸ�");// ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }
    private void ExitGuardEvent()
    {
        if (isGuard) ExitGuard();
    }

    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < eSkillCooldown) return; // E ��ų ��Ÿ�� üũ
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

        while (Time.time - lastEActionTime < eSkillCooldown)
        {
            Debug.Log($"E��ų ���� �ð� : {lastEActionTime}"); // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        Debug.Log($"E��ų ��Ÿ�� �Ϸ�"); // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }
}

