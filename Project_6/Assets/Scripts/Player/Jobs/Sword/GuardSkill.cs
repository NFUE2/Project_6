using UnityEngine;
using TMPro;

public class GuardSkill : SkillBase, IDamagable
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 1.0f; // 가드 지속 시간
    public float DefenseBoostDuringGuard = 50f; // 가드 중 방어력 증가량
    public PlayerDataSO PlayerData;
    public float DamageReduction;

    private Animator animator;
    private PlayerCondition playerCondition;
    private float originalDamageReduction;

    public AudioClip guardSound; // 방어 성공 시 효과음 추가
    private AudioSource audioSource; // AudioSource 컴포넌트 추가

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
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

            Debug.Log("Q 스킬 사용");
            EnterGuard();
        }
    }

    private void EnterGuard()
    {
        IsGuard = true;
        SaveOriginalStats();
        ApplyGuardStats();
        animator.SetBool("IsGuard", true); // 가드 애니메이션 시작
        Invoke("ExitGuardEvent", GuardDuration); // 가드 시간 이벤트 설정
    }

    private void ExitGuard()
    {
        Debug.Log("가드 종료");
        IsGuard = false;
        RestoreOriginalStats();
        animator.SetBool("IsGuard", false); // 가드 애니메이션 종료
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
