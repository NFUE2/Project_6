using UnityEngine;

public class GuardSkill : SkillBase, IDamagable
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 3.0f;
    public float DefenseBoostDuringGuard = 50f;
    public PlayerDataSO PlayerData;

    public AudioClip guardSound;
    private AudioSource audioSource;
    private PlayerCondition playerCondition;

    // 파티클을 생성할 위치
    public Transform particleSpawnPoint;
    // 사용할 파티클 프리팹
    public GameObject guardParticlePrefab;

    // 현재 생성된 파티클 오브젝트
    private GameObject activeGuardParticle;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        playerCondition = GetComponent<PlayerCondition>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void UseSkill()
    {
        if (IsGuard)
        {
            ExitGuard();
        }
        else
        {
            if (Time.time - lastActionTime < cooldownDuration) return;
            EnterGuard();
        }
    }

    private void EnterGuard()
    {
        IsGuard = true;
        ApplyGuardStats();
        CreateGuardParticle();  // 파티클 생성

        Invoke("ExitGuardEvent", GuardDuration);
    }

    private void ExitGuard()
    {
        IsGuard = false;
        RestoreOriginalStats();
        DestroyGuardParticle();  // 파티클 파괴
        lastActionTime = Time.time;
    }

    private void ExitGuardEvent()
    {
        if (IsGuard) ExitGuard();
    }

    private void ApplyGuardStats()
    {
        playerCondition.ModifyDefense(DefenseBoostDuringGuard);
    }

    private void RestoreOriginalStats()
    {
        playerCondition.ModifyDefense(-DefenseBoostDuringGuard);
    }

    public void PlayGuardSound()
    {
        if (guardSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(guardSound);
        }
    }

    // 파티클 오브젝트를 생성하는 메서드
    private void CreateGuardParticle()
    {
        if (guardParticlePrefab != null && particleSpawnPoint != null)
        {
            activeGuardParticle = Instantiate(guardParticlePrefab, particleSpawnPoint.position, particleSpawnPoint.rotation, particleSpawnPoint);
        }
    }

    // 파티클 오브젝트를 파괴하는 메서드
    private void DestroyGuardParticle()
    {
        if (activeGuardParticle != null)
        {
            Destroy(activeGuardParticle);
        }
    }

    public void TakeDamage(float damage)
    {
        if (IsGuard)
        {
            PlayGuardSound();
        }
        else
        {
            playerCondition.TakeDamage(damage);
        }
    }
}
