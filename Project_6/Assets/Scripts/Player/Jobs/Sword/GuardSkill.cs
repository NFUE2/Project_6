using UnityEngine;

public class GuardSkill : SkillBase, IDamagable
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 3.0f;
    public float DefenseBoostDuringGuard = 50f;
    public PlayerDataSO PlayerData;

    public GameObject guardParticleEffectPrefab;
    public AudioClip guardSound;
    private AudioSource audioSource;
    private PlayerCondition playerCondition;

    private GameObject guardParticleEffectInstance;

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

        if (guardParticleEffectPrefab != null)
        {
            guardParticleEffectInstance = Instantiate(guardParticleEffectPrefab, transform.position, Quaternion.identity);
            guardParticleEffectInstance.transform.SetParent(transform);
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
