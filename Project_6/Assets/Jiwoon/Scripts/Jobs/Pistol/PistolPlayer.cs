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
    private float cooldownDuration = 3f; //장전을 위한 쿨타임

    // 수정 사항 - 캐릭터 공통부분은 PlayerBase로 올려주세요
    private float attackTime;
    private float lastAttackTime;
    //=====================================================

    private Camera mainCamera;
    private bool isRolling;
    private bool fanningReady;
    public Transform attackPoint; //발사위치
    public GameObject attackPrefab; //발사할 오브젝트 에셋

    // 수정 사항 - 캐릭터 공통부분은 PlayerBase로 올려주세요
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
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // 마우스의 위치값
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // 마우스의 위치값에서 지정해준 공격 시작 위치값을 뺀다 => 공격 방향
        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // 총알을 생성해 발사해 공격한다.
        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity); // 총알을 생성해 발사해 공격한다.

        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // 공격 속도 설정 한다.

        if (attackCount >= 6)
        {
            StartCoroutine(AttackCooldown());
        }
    }
    public IEnumerator AttackCooldown() //6발을 쓰고 장전을 한다.
    {
        isAttackCooldown = true;
        attackCount = 0;
        yield return new WaitForSeconds(cooldownDuration);
        isAttackCooldown = false;
    }

    public override void UseSkillQ()
    {
        //재장전 스크립트 필요
        if (fanningReady) return;
        if (Time.time - lastAction < actionTime) return;

        fanningReady = GetComponent<PlayerController_Gun>().fanningReady = true;
        //StartCoroutine(Fanning());
    }

    public override void UseSkillE()
    {
        // 권총의 E 스킬 로직
    }
}
