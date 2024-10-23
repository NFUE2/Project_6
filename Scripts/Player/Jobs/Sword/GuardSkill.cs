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

    // ��ƼŬ�� ������ ��ġ
    public Transform particleSpawnPoint;
    // ����� ��ƼŬ ������
    public GameObject guardParticlePrefab;

    // ���� ������ ��ƼŬ ������Ʈ
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
        CreateGuardParticle();  // ��ƼŬ ����

        Invoke("ExitGuardEvent", GuardDuration);
    }

    private void ExitGuard()
    {
        IsGuard = false;
        RestoreOriginalStats();
        DestroyGuardParticle();  // ��ƼŬ �ı�
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

    // ��ƼŬ ������Ʈ�� �����ϴ� �޼���
    private void CreateGuardParticle()
    {
        if (guardParticlePrefab != null && particleSpawnPoint != null)
        {
            activeGuardParticle = Instantiate(guardParticlePrefab, particleSpawnPoint.position, particleSpawnPoint.rotation, particleSpawnPoint);
        }
    }

    // ��ƼŬ ������Ʈ�� �ı��ϴ� �޼���
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
