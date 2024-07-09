using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;

public class SwordPlayer : PlayerBase
{

    [Header("Animation Data")]
    public Animator animator; // 향후 애니메이션 에셋 추가 => Sword를 위한 애니메이션 컨트롤러

    [Header("Skill Q")]
    private bool isGuard;
    private float qSkillCooldown;
    private float lastQActionTime;

    [Header("Skill E")]
    public GameObject projectile;  //Sword 플레이어가 쏘는 오브젝트다. 향후 에셋 추가
    private float eSkillCooldown;
    private float lastEActionTime;

    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;

    public override void Attack()
    {
        if (isGuard) return; // 가드 상태에서는 공격 불가
        if (Time.time - lastAttackTime < attackTime) return; // 공격 딜레이 체크
        Debug.Log("일반공격!");
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
            if (Time.time - lastQActionTime < qSkillCooldown) return; // Q 스킬 쿨타임 체크

            Debug.Log("Q 스킬 사용");
            isGuard = true;
            //animator.SetBool("Guard", true);
            Invoke("ExitGuardEvent", 1.0f);
        }
    }

    private void ExitGuard()
    {
        Debug.Log("가드 종료");
        isGuard = false;
        //animator.SetBool("Guard", false);
        StartCoroutine(CoolTimeQ());
    }

    private IEnumerator CoolTimeQ()
    {
        lastQActionTime = Time.time;

        while (Time.time - lastQActionTime < qSkillCooldown)
        {
            Debug.Log($"Q스킬 남은 시간 : {lastQActionTime}"); // 쿨타임 텍스트 갱신
            yield return null;
        }
        Debug.Log($"Q스킬 쿨타임 완료");// 쿨타임 완료 텍스트 갱신
    }
    private void ExitGuardEvent()
    {
        if (isGuard) ExitGuard();
    }

    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < eSkillCooldown) return; // E 스킬 쿨타임 체크
        Debug.Log("E 스킬 사용");
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //향후 projectile 에셋 추가 할때 주석 풀기
        //GameObject go = PhotonNetwork.Instantiate("Prototype/" + projectile.name, transform.position, Quaternion.identity);
        //go.transform.localEulerAngles = new Vector3(0, 0, angle);

        StartCoroutine(CoolTimeE());
    }

    private IEnumerator CoolTimeE()
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

