using UnityEngine;

public class GuardSkill : SkillBase, IDamagable
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 3.0f;
    public float DefenseBoostDuringGuard = 50f;
    public PlayerDataSO PlayerData;
    public float DamageReduction;

    public GameObject guardParticleEffectPrefab;
    public AudioClip guardSound;
    private AudioSource audioSource;
    private PlayerCondition playerCondition;
    private float originalDamageReduction;

    private GameObject guardParticleEffectInstance;

    public Material outlineMaterial; // 윤곽선 셰이더 머티리얼
    private Material originalMaterial;
    private Renderer playerRenderer;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        playerCondition = GetComponent<PlayerCondition>();
        audioSource = GetComponent<AudioSource>();
        playerRenderer = GetComponent<Renderer>(); // 플레이어의 렌더러 가져오기
        originalMaterial = playerRenderer.material; // 원래 머티리얼 저장
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
        SaveOriginalStats();
        ApplyGuardStats();

        if (guardParticleEffectPrefab != null)
        {
            guardParticleEffectInstance = Instantiate(guardParticleEffectPrefab, transform.position, Quaternion.identity);
            guardParticleEffectInstance.transform.SetParent(transform);
        }

        // 윤곽선 셰이더 적용
        if (outlineMaterial != null)
        {
            playerRenderer.material = outlineMaterial;
        }

        Invoke("ExitGuardEvent", GuardDuration);
    }

    private void ExitGuard()
    {
        IsGuard = false;
        RestoreOriginalStats();

        if (guardParticleEffectInstance != null)
        {
            Destroy(guardParticleEffectInstance);
        }

        // 원래 셰이더로 복원
        playerRenderer.material = originalMaterial;

        lastActionTime = Time.time;
    }

    private void ExitGuardEvent()
    {
        if (IsGuard) ExitGuard();
    }

    private void SaveOriginalStats()
    {
        originalDamageReduction = DamageReduction;
    }

    private void ApplyGuardStats()
    {
        playerCondition.ModifyDefense(DefenseBoostDuringGuard);
    }

    private void RestoreOriginalStats()
    {
        playerCondition.ModifyDefense(-DefenseBoostDuringGuard);
        DamageReduction = originalDamageReduction;
    }

    public void PlayGuardSound()
    {
        if (guardSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(guardSound);
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
