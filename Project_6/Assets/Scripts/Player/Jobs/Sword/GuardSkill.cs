using UnityEngine;

public class GuardSkill : SkillBase, IDamagable
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 3.0f; // 가드 지속 시간
    public float DefenseBoostDuringGuard = 50f; // 가드 중 방어력 증가량
    public PlayerDataSO PlayerData;
    public float DamageReduction;

    public GameObject guardParticleEffectPrefab; // 파티클 효과 프리팹
    public AudioClip guardSound; // 방어 성공 시 효과음 추가
    private AudioSource audioSource; // AudioSource 컴포넌트
    private PlayerCondition playerCondition;
    private float originalDamageReduction;

    private GameObject guardParticleEffectInstance; // 생성된 파티클 오브젝트 인스턴스

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        playerCondition = GetComponent<PlayerCondition>(); // PlayerCondition 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
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

        if (guardParticleEffectPrefab != null)
        {
            // 파티클 효과 생성 및 플레이어 위치에 맞게 배치
            guardParticleEffectInstance = Instantiate(guardParticleEffectPrefab, transform.position, Quaternion.identity);
            guardParticleEffectInstance.transform.SetParent(transform); // 플레이어를 부모로 설정

            // 플레이어의 중심에 위치시킴
            var playerRenderer = GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                guardParticleEffectInstance.transform.localPosition = playerRenderer.bounds.center - transform.position;
            }
            else
            {
                // renderer가 없는 경우, transform 중심에 위치
                guardParticleEffectInstance.transform.localPosition = Vector3.zero;
            }
        }

        Invoke("ExitGuardEvent", GuardDuration); // 가드 지속 시간 후 이벤트 설정
    }

    private void ExitGuard()
    {
        IsGuard = false;
        RestoreOriginalStats();

        if (guardParticleEffectInstance != null)
        {
            Destroy(guardParticleEffectInstance); // 파티클 오브젝트 제거
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
            // 방어 중일 때 데미지 무시
        }
        else
        {
            // 방어 중이 아닐 때는 기본 데미지 처리 로직 호출
            playerCondition.TakeDamage(damage);
        }
    }
}
