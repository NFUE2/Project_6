using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using TMPro;

public class DaggerPlayer : PlayerBase
{
    public PlayerData PlayerData;

    public TextMeshPro qCooldownText; // Q 스킬 쿨타임을 표시하는 UI 텍스트 요소
    public TextMeshPro eCooldownText; // E 스킬 쿨타임을 표시하는 UI 텍스트 요소

    [Header("Animation Data")]
    public Animator animator; // 향후 애니메이션 에셋 추가 => Dagger를 위한 애니메이션 컨트롤러

    //스킬클래스로 이동 - 만약 스킬클래스에서 처리 못하면 말해주세요
    [Header("Skill Q")]
    public float dashDistance = 5f; // 대쉬 거리
    public float dashSpeed = 10f; // 대쉬 속도
    public LayerMask enemyLayer; // 적 레이어 마스크

    [Header("Skill E")]
    public int currentStack = 0; // 현재 스택 수
    public int maxStack = 10; // 최대 스택 수
    public int damagePerStack = 10; // 스택 당 데미지
    public TextMeshPro stackText; // 스택을 표시하는 UI 텍스트 요소
    //====================================

    //공격부분 - 상위클래스로 이동
    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;
    //====================================

    //근접 캐릭터 클래스에서 구현 - 필요하면 새 스크립트 작성
    public override void Attack()
    {
        if (Time.time - lastAttackTime < attackTime) return; // 공격 딜레이 체크
        Debug.Log("일반공격!");
        lastAttackTime = Time.time;
        //animator.SetTrigger("Attack");

        // 공격이 적중했다고 가정
        IncreaseStack();
    }

    //스킬클래스에서 구현
    private void IncreaseStack() //E스킬을 쓰기위한 스텍
    {
        currentStack++;
        if (currentStack > maxStack)
        {
            currentStack = maxStack;
        }
        stackText.text = $"스택: {currentStack}"; // 스택 UI 갱신
    }
    public override void UseSkillQ()
    {
        StartCoroutine(Dash());
        StartCoroutine(CoolTimeQ());
    }
    private IEnumerator Dash()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + transform.forward * dashDistance;
        float startTime = Time.time;

        while (Time.time < startTime + (dashDistance / dashSpeed))
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (Time.time - startTime) * dashSpeed / dashDistance);
            yield return null;
        }

        transform.position = endPosition;
        DealDamageToEnemiesOnPath(startPosition, endPosition);
    }

    private void DealDamageToEnemiesOnPath(Vector3 startPosition, Vector3 endPosition)
    {
        RaycastHit[] hits = Physics.RaycastAll(startPosition, endPosition - startPosition, dashDistance, enemyLayer);
        foreach (var hit in hits)
        {
            // 여기에 적에게 데미지를 입히는 코드를 추가
            Debug.Log($"적 {hit.collider.name}에게 데미지!");
        }
    }
    private IEnumerator CoolTimeQ()
    {
        lastQActionTime = Time.time;

        while (Time.time - lastQActionTime < PlayerData.SkillQCooldown)
        {
            float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastQActionTime);
            qCooldownText.text = $"Q스킬 남은 시간: {remainingTime:F1}초"; // 쿨타임 텍스트 갱신
            yield return null;
        }
        qCooldownText.text = "Q스킬 쿨타임 완료"; // 쿨타임 완료 텍스트 갱신
    }

    public override void UseSkillE()
    {
        if (currentStack > 0)
        {
            DealDamageWithStack();
            currentStack = 0; // 스택 리셋
            stackText.text = "스택: 0"; // 스택 UI 갱신
            StartCoroutine(CoolTimeE());
        }
        else
        {
            Debug.Log("스택이 부족합니다."); // 스택이 부족할 때 메시지
        }
    }
    private void DealDamageWithStack()
    {
        // 적에게 데미지를 입히는 로직
        int totalDamage = currentStack * damagePerStack;
        Debug.Log($"스택 {currentStack}개를 사용하여 {totalDamage}의 데미지를 입혔습니다.");

        // 애니메이션 트리거 (칼 내려침)
        // animator.SetTrigger("Slash");

        // 여기서 적에게 데미지를 입히는 코드를 추가하세요.
    }

    private IEnumerator CoolTimeE()
    {
        lastEActionTime = Time.time;

        while (Time.time - lastEActionTime < PlayerData.SkillECooldown)
        {
            float remainingTime = PlayerData.SkillECooldown - (Time.time - lastEActionTime);
            eCooldownText.text = $"E스킬 남은 시간: {remainingTime:F1}초"; // 쿨타임 텍스트 갱신
            yield return null;
        }
        eCooldownText.text = "E스킬 쿨타임 완료"; // 쿨타임 완료 텍스트 갱신
    }
    //================================
}

