using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using TMPro;

public class SwordPlayer : PlayerBase
{
    public PlayerData PlayerData;
    public TextMeshProUGUI qCooldownText; // Q 스킬 쿨타임을 표시하는 UI 텍스트 요소
    public TextMeshProUGUI eCooldownText; // E 스킬 쿨타임을 표시하는 UI 텍스트 요소

    [Header("Animation Data")]
    public Animator animator; // 향후 애니메이션 에셋 추가 => Sword를 위한 애니메이션 컨트롤러


    //스킬클래스로 이동 - 만약 스킬클래스에서 처리 못하면 말해주세요
    [Header("Skill Q")]
    private bool isGuard;

    [Header("Skill E")]
    public GameObject projectile;  //Sword 플레이어가 쏘는 오브젝트다. 향후 에셋 추가
    //===================================

    //공격부분 - 상위클래스로 이동
    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;
    //====================================

    //임시 공격 수치
    public float attackRange = 2.0f; // 공격 범위 추가
    public int attackDamage = 10; // 공격 데미지 추가
    public LayerMask enemyLayer; // 적 레이어 추가


    //
    public override void Attack()
    {
        if (isGuard) return; // 가드 상태에서는 공격 불가
        if (Time.time - lastAttackTime < attackTime) return; // 공격 딜레이 체크
        Debug.Log("일반공격!");
        lastAttackTime = Time.time;
        //animator.SetTrigger("Attack");

        //임시 일반공격
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            enemy.GetComponent<IDamagable>().TakeDamage(attackDamage);
        }
        //
    }
    //임시공격보이는 판정
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    

    //스킬클래스에서 구현
    public override void UseSkillQ()
    {
        if (isGuard)
        {
            ExitGuard();
        }
        else
        {
            if (Time.time - lastQActionTime < PlayerData.SkillQCooldown) return; // Q 스킬 쿨타임 체크

            Debug.Log("Q 스킬 사용");
            isGuard = true;
            //animator.SetBool("Guard", true);
            Invoke("ExitGuardEvent", 1.0f); //토글스킬임, 가드시간 데이터로 빼야함
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

        while (Time.time - lastQActionTime < PlayerData.SkillQCooldown)
        {
            float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastQActionTime);
            qCooldownText.text = $"{remainingTime:F1}"; // 쿨타임 텍스트 갱신
            yield return null;
        }
        qCooldownText.text = "Q스킬 쿨타임 완료"; // 쿨타임 완료 텍스트 갱신
    }
    private void ExitGuardEvent()
    {
        if (isGuard) ExitGuard();
    }

    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < PlayerData.SkillECooldown) return; // E 스킬 쿨타임 체크
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

        while (Time.time - lastEActionTime < PlayerData.SkillECooldown)
        {
            float remainingTime = PlayerData.SkillECooldown - (Time.time - lastEActionTime);
            eCooldownText.text = $"{remainingTime:F1}"; // 쿨타임 텍스트 갱신
            yield return null;
        }
        eCooldownText.text = "E스킬 쿨타임 완료"; // 쿨타임 완료 텍스트 갱신
    }
    //==========================
}

