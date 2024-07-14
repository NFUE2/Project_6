using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class BowPlayer : PlayerBase
{
    [Header("Attack")]
    private bool isAttackCooldown = false; 
    private int attackCount = 0;

    //공격부분 - 상위클래스로 이동
    private float attackTime;
    private float lastAttackTime;
    //====================================

    
    private Camera mainCamera;

    public GameObject attackPrefab; //총알
    public Transform attackPoint;
    private float cooldownDuration = 0.5f;

    //스킬클래스로 이동 - 만약 스킬클래스에서 처리 못하면 말해주세요
    [Header("Skill Q")]
    public GameObject wireArrow;

    [Header("Skill E")]
    public GameObject bombArrow;
    public float fireAngle;
    //====================================

    //원거리 캐릭터에서 구현
    public override void Attack()
    {
        if (isAttackCooldown) return;

        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        attackCount++;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // 마우스의 위치값
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // 마우스의 위치값에서 지정해준 공격 시작 위치값을 뺀다 => 공격 방향
        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // 총알을 생성해 발사해 공격한다.
        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity); // 총알을 생성해 발사해 공격한다.

        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // 공격 속도 설정 한다.

        //lastAttackTime의 의도동일,제거
        if (attackCount >= 1)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    //삭제
    private IEnumerator AttackCooldown() //발사속도
    {
        isAttackCooldown = true;
        attackCount = 0;
        yield return new WaitForSeconds(cooldownDuration);
        isAttackCooldown = false;
    }


    //스킬클래스에서 구현
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
            Debug.Log($"E스킬 남은 시간 : {lastEActionTime}"); // 쿨타임 텍스트 갱신
            yield return null;
        }
        Debug.Log($"E스킬 쿨타임 완료"); // 쿨타임 완료 텍스트 갱신
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
            Debug.Log($"E스킬 남은 시간 : {lastEActionTime}"); // 쿨타임 텍스트 갱신
            yield return null;
        }
        Debug.Log($"E스킬 쿨타임 완료"); // 쿨타임 완료 텍스트 갱신
    }
    //========================================
}