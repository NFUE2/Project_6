using UnityEngine;
using TMPro;

public class GuardSkill : SkillBase, IDamagable
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 3.0f; // 가드 지속 시간
    public float DefenseBoostDuringGuard = 50f; // 가드 중 방어력 증가량
    public PlayerDataSO PlayerData;
    public float DamageReduction;

    public GameObject guardParticleEffectObject; // 파티클 효과가 포함된 게임 오브젝트
    public AudioClip guardSound; // 방어 성공 시 효과음 추가
    private AudioSource audioSource; // AudioSource 컴포넌트 추가
    private PlayerCondition playerCondition;
    private float originalDamageReduction;

    private ParticleSystem guardParticleSystem;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        playerCondition = GetComponent<PlayerCondition>(); // PlayerCondition 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기

        if (guardParticleEffectObject != null)
        {
            guardParticleSystem = guardParticleEffectObject.GetComponent<ParticleSystem>();
            if (guardParticleSystem != null)
            {
                var mainModule = guardParticleSystem.main;
                mainModule.loop = false; // 파티클 루프 해제
                mainModule.startLifetime = GuardDuration; // 파티클의 수명을 가드 지속 시간과 일치시킴
            }

            guardParticleEffectObject.SetActive(false); // 초기에는 파티클 오브젝트 비활성화
        }
    }

    public override void UseSkill()
    {
        if (IsGuard)
        {
            ExitGuard();
        }
        else
        {
            if (Time.time - lastActionTime < cooldownDuration) return; // Q 스킬 쿨타임 체크
            EnterGuard();
        }
    }

    private void EnterGuard()
    {
        IsGuard = true;
        SaveOriginalStats();
        ApplyGuardStats();

        if (guardParticleEffectObject != null)
        {
            guardParticleEffectObject.SetActive(true); // 파티클 효과 게임 오브젝트 활성화
            guardParticleSystem.Play(); // 파티클 재생
        }

        Invoke("ExitGuardEvent", GuardDuration); // 가드 시간 이벤트 설정
    }

    private void ExitGuard()
    {
        IsGuard = false;
        RestoreOriginalStats();

        if (guardParticleEffectObject != null)
        {
            guardParticleSystem.Stop(); // 파티클 정지
            guardParticleEffectObject.SetActive(false); // 파티클 효과 게임 오브젝트 비활성화
        }

        lastActionTime = Time.time;
    }

    private void ExitGuardEvent()
    {
        if (IsGuard) ExitGuard();
    }

    private void SaveOriginalStats()
    {
        originalDamageReduction = DamageReduction; // 현재 데미지 감소 비율 저장
    }

    private void ApplyGuardStats()
    {
        playerCondition.ModifyDefense(DefenseBoostDuringGuard); // 방어력 증가 적용
    }

    private void RestoreOriginalStats()
    {
        playerCondition.ModifyDefense(-DefenseBoostDuringGuard); // 방어력 복원
        DamageReduction = originalDamageReduction; // 데미지 감소 비율 복원
    }

    public void PlayGuardSound()
    {
        if (guardSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(guardSound); // 방어 성공 시 효과음 재생
        }
    }

    public void TakeDamage(float damage)
    {
        if (IsGuard)
        {
            // 방어 중일 때 데미지 효과음 재생
            PlayGuardSound();
            // 데미지를 무시할 수 있지만, 여기서 데미지 처리 로직을 추가할 수 있음
        }
        else
        {
            // 방어 중이 아닐 때는 기본 데미지 처리 로직을 호출하거나 처리할 수 있음
            playerCondition.TakeDamage(damage); // 이 부분은 인터페이스 구현에 따라 다름
        }
    }
}
