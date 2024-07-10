using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using Photon.Pun.Demo.Asteroids;

public class PistolPlayer : PlayerBase
{
    [Header("Attack")]
    private bool isAttackCooldown = false;
    private int attackCount = 0;
    private float cooldownDuration = 3f; //장전속도
    public GameObject attackPrefab; //총알
    private float attackTime;
    private float lastAttackTime;

    [Header("Skill Q")]
    private Camera mainCamera;
    private bool isRolling;
    private bool fanningReady;
    public Transform attackPoint; //발사 위치

    [Header("Skill E")]
    //Animator animator;
    Rigidbody2D rigidbody;
    public float rollingX;
    //public bool isRolling { get; private set; }
    public bool isInvincible { get; private set; }
    //====================================================

    public bool fanningReadyQ;

    public override void Attack()
    {
        if (isAttackCooldown) return;
        if (fanningReady || isRolling) return;

        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        attackCount++;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;
        GameObject attackInstance = PhotonNetwork.Instantiate(attackPrefab.name, attackPoint.position, Quaternion.identity);

        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f;

        if (attackCount >= 6)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    public IEnumerator AttackCooldown()
    {
        isAttackCooldown = true;
        attackCount = 0;
        yield return new WaitForSeconds(cooldownDuration);
        isAttackCooldown = false;
    }

    public override void UseSkillQ()
    {
        if (fanningReady) return;
        if (Time.time - lastQActionTime < qSkillCooldown) return;

        fanningReady = true;
        StartCoroutine(Fanning());
    }

    IEnumerator Fanning()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;

        for (int i = 0; i < 6; i++)
        {
            float fireAngle = Random.Range(-3f, 3f);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            Debug.Log(mousePos);

            GameObject go = PhotonNetwork.Instantiate(attackPrefab.name, transform.position, Quaternion.identity);
            go.transform.localEulerAngles = new Vector3(0, 0, angle + fireAngle);

            yield return new WaitForSeconds(0.1f);
        }

        fanningReady = false;
        StartCoroutine(AttackCooldown());
        StartCoroutine(CoolTimeQ());
    }

    IEnumerator CoolTimeQ()
    {
        lastQActionTime = Time.time;

        while (Time.time - lastQActionTime < qSkillCooldown)
        {
            Debug.Log($"Q스킬 남은 시간 : {qSkillCooldown - (Time.time - lastQActionTime)}"); // 쿨타임 텍스트 갱신
            yield return null;
        }
        Debug.Log($"Q스킬 쿨타임 완료"); // 쿨타임 완료 텍스트 갱신
    }

    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < eSkillCooldown) return; // E 스킬 쿨타임 체크

        if (GetComponent<PlayerController_Gun>().isRolling) return;
        GetComponent<PlayerController_Gun>().isRolling = true;

        isInvincible = true;

        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        int c = dir.x > 0 ? 1 : -1;

        GetComponent<Rigidbody>().velocity = new Vector2(rollingX * c, 0);

        Invoke("ExitRolling", 1.0f);
    }

    public void ExitRolling()
    {
        GetComponent<PlayerController_Gun>().isRolling = false;
        isInvincible = false;
        rigidbody.velocity = Vector2.zero;
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
}
