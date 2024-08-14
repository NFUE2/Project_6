using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Collections;

public class PistolPlayer : RangedPlayerBase
{
    private int attackCount = 0;
    private float cooldownDuration = 2f;

    [Header("Skill Q")]
    [SerializeField] private FanningSkill fanningSkill;

    [Header("Skill E")]
    [SerializeField] private RollingSkill rollingSkill;

    [Header("Reload Sound")]
    [SerializeField] private AudioClip reloadSound; // 장전 효과음

    private bool isUsingSkill = false;
    private bool isReloading = false; // 장전 중인지 확인하는 변수

    private void Start()
    {
        fanningSkill.SetCooldownImage(qCooldownImage);
        rollingSkill.SetCooldownImage(eCooldownImage);

        // 오디오 소스 컴포넌트 가져오기 또는 추가하기
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    public override void Attack()
    {
        if (currentAttackTime >= playerData.attackTime && !isUsingSkill && !isReloading)
        {
            base.Attack(); // 기본 공격 로직 호출

            attackCount++;

            if (attackCount >= 6)
            {
                StartReload();
            }
            else
            {
                currentAttackTime = 0f; // 쿨타임 초기화
            }
        }
    }

    private void Update()
    {
        UpdateAttackCooldown();
    }

    private void UpdateAttackCooldown()
    {
        if (!isReloading) // 장전 중이 아닐 때만 쿨타임을 업데이트
        {
            if (currentAttackTime < playerData.attackTime)
            {
                currentAttackTime += Time.deltaTime;
                if (attackCooldownbar != null)
                {
                    attackCooldownbar.fillAmount = currentAttackTime / playerData.attackTime;
                }
            }
        }
        else
        {
            // 장전 중에는 공격 바가 차지 않도록 설정
            if (attackCooldownbar != null)
            {
                attackCooldownbar.fillAmount = 0f;
            }
        }
    }

    public override void UseSkillQ()
    {
        if (!isUsingSkill && !isReloading)
        {
            isUsingSkill = true;
            fanningSkill.UseSkill();
            StartCoroutine(HandleFanningSkillCompletion());
        }
    }

    public override void UseSkillE()
    {
        if (!isUsingSkill && !isReloading)
        {
            rollingSkill.UseSkill();
        }
    }

    private IEnumerator HandleFanningSkillCompletion()
    {
        while (fanningSkill.IsFanningReady)
        {
            yield return null;
        }

        attackCount = 0;
        currentAttackTime = cooldownDuration; // 스킬 완료 시 쿨타임 초기화
        isUsingSkill = false;
    }

    private void StartReload()
    {
        isReloading = true;
        PlayReloadSound(); // 장전 시작 시 장전 효과음 재생
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(cooldownDuration); // 장전 시간 기다리기
        isReloading = false;
        attackCount = 0; // 공격 카운트 초기화
        currentAttackTime = 0f; // 공격 쿨타임 초기화
    }

    private void PlayReloadSound()
    {
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
    }
}
