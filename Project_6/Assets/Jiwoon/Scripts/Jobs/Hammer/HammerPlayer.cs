using Photon.Pun;
using UnityEngine;
using System.Collections;

public class HammerPlayer : PlayerBase
{
    //���ݺκ� - ����Ŭ������ �̵�
    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;
    //====================================

    [Header("Animation Data")]
    public Animator animator; // ���� �ִϸ��̼� ���� �߰� => Sword�� ���� �ִϸ��̼� ��Ʈ�ѷ�

    //��ųŬ������ �̵� - ���� ��ųŬ�������� ó�� ���ϸ� �����ּ���
    [Header("Skill Q")]
    public GameObject shield;
    private GameObject createShield;
    //public float shieldTime;

    [Header("Skill E")]
    public float maxChargingTime;
    public bool isCharging;
    private bool isSkillAttack; // ��ų ���� ���θ� ��Ÿ���� �÷���
    public float damage;
    public float damageRate;
    //====================================

    //���� ĳ���� Ŭ�������� ����
    public override void Attack()
    {
        if (isCharging) return;
        if (Time.time - lastAttackTime < attackTime) return;

        lastAttackTime = Time.time;
        // ���� �ִϸ��̼� ���
        animator.SetTrigger("Attack");
    }


    //��ų Ŭ�������� ����
    public override void UseSkillQ()
    {
        if (createShield != null) return;
        //if (Time.time - lastQActionTime < qSkillCooldown) return;
        //createShield = Instantiate(shield, transform.position, Quaternion.identity);
        createShield = PhotonNetwork.Instantiate("Prototype/" + shield.name, transform.position, Quaternion.identity);
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        createShield.transform.localEulerAngles = new Vector3(0, 0, angle);

        //Invoke("ShieldDestroy", shieldTime);
        StartCoroutine(CoolTimeQ());
    }
    private void Update()
    {
        if (createShield != null && createShield.activeInHierarchy) createShield.transform.position = transform.position;
    }

    //private void ShieldDestroy()
    //{
    //    if (createShield != null) Destroy(createShield);
    //}
    IEnumerator CoolTimeQ()
    {
        lastQActionTime = Time.time;

        //while (Time.time - lastQActionTime < qSkillCooldown)
        {
            Debug.Log($"Q��ų ���� �ð� : {lastQActionTime}"); // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        Debug.Log($"Q��ų ��Ÿ�� �Ϸ�");// ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }

    public override void UseSkillE()
    {
        if (isCharging) return;
        //if (Time.time - lastEActionTime < eSkillCooldown) return;

        animator.SetBool("Charging", isCharging = true);
        StartCoroutine(Charging());
    }
    IEnumerator Charging()
    {
        animator.SetBool("Charging", isCharging);
        float startCharging = Time.time;
        float currentDamage = damage;
        while (!Input.GetKeyUp(KeyCode.E) && !(Time.time - startCharging > maxChargingTime))
        {
            currentDamage += Time.deltaTime * damageRate;
            yield return null;
        }
        animator.SetBool("Charging", isCharging = false);
        isSkillAttack = true; // ��ų ���� �÷��� ����
        StartCoroutine(CoolTimeE());
        Smash(currentDamage);
    }

    public void Smash(float currentDamage)
    {
        // ��ų ���� �������� ����
        this.currentDamage = currentDamage;
    }

    private float currentDamage;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log($"�浹�� ��ü: {collider.name}");
        P_Tentaclypse boss = collider.GetComponent<P_Tentaclypse>();
        if (boss != null)
        {
            if (isSkillAttack)
            {
                Debug.Log($"��ų ������ {currentDamage} ��ŭ �־����ϴ�");
                boss.TakeDamage(currentDamage);
                isSkillAttack = false; // ��ų ���� �÷��� �ʱ�ȭ
            }
            else
            {
                Debug.Log($"�Ϲ� ���� ������ {damage} ��ŭ �־����ϴ�");
                boss.TakeDamage(damage);
            }
        }
        else
        {
            //Debug.Log($"�� ������Ʈ�� �ƴմϴ�: {collider.name}");
        }
        currentDamage = damage; // ������ �ʱ�ȭ
    }

    IEnumerator CoolTimeE()
    {
        lastEActionTime = Time.time;

        //while (Time.time - lastEActionTime < eSkillCooldown)
        {
            Debug.Log($"E��ų ���� �ð� : {lastEActionTime}"); // ��Ÿ�� �ؽ�Ʈ ����
            yield return null;
        }
        Debug.Log($"E��ų ��Ÿ�� �Ϸ�"); // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
    }
    //=========================================
}
