using UnityEngine;

public class GuardSkill : SkillBase, IDamagable
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 3.0f; // ���� ���� �ð�
    public float DefenseBoostDuringGuard = 50f; // ���� �� ���� ������
    public PlayerDataSO PlayerData;
    public float DamageReduction;

    public GameObject guardParticleEffectPrefab; // ��ƼŬ ȿ�� ������
    public AudioClip guardSound; // ��� ���� �� ȿ���� �߰�
    private AudioSource audioSource; // AudioSource ������Ʈ
    private PlayerCondition playerCondition;
    private float originalDamageReduction;

    private GameObject guardParticleEffectInstance; // ������ ��ƼŬ ������Ʈ �ν��Ͻ�

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        playerCondition = GetComponent<PlayerCondition>(); // PlayerCondition ������Ʈ ��������
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ��������
    }

    public override void UseSkill()
    {
        if (IsGuard)
        {
            ExitGuard();
        }
        else
        {
            if (Time.time - lastActionTime < cooldownDuration) return; // Q ��ų ��Ÿ�� üũ
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
            // ��ƼŬ ȿ�� ���� �� �÷��̾� ��ġ�� �°� ��ġ
            guardParticleEffectInstance = Instantiate(guardParticleEffectPrefab, transform.position, Quaternion.identity);
            guardParticleEffectInstance.transform.SetParent(transform); // �÷��̾ �θ�� ����

            // �÷��̾��� �߽ɿ� ��ġ��Ŵ
            var playerRenderer = GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                guardParticleEffectInstance.transform.localPosition = playerRenderer.bounds.center - transform.position;
            }
            else
            {
                // renderer�� ���� ���, transform �߽ɿ� ��ġ
                guardParticleEffectInstance.transform.localPosition = Vector3.zero;
            }
        }

        Invoke("ExitGuardEvent", GuardDuration); // ���� ���� �ð� �� �̺�Ʈ ����
    }

    private void ExitGuard()
    {
        IsGuard = false;
        RestoreOriginalStats();

        if (guardParticleEffectInstance != null)
        {
            Destroy(guardParticleEffectInstance); // ��ƼŬ ������Ʈ ����
        }

        lastActionTime = Time.time;
    }

    private void ExitGuardEvent()
    {
        if (IsGuard) ExitGuard();
    }

    private void SaveOriginalStats()
    {
        originalDamageReduction = DamageReduction; // ���� ������ ���� ���� ����
    }

    private void ApplyGuardStats()
    {
        playerCondition.ModifyDefense(DefenseBoostDuringGuard); // ���� ���� ����
    }

    private void RestoreOriginalStats()
    {
        playerCondition.ModifyDefense(-DefenseBoostDuringGuard); // ���� ����
        DamageReduction = originalDamageReduction; // ������ ���� ���� ����
    }

    public void PlayGuardSound()
    {
        if (guardSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(guardSound); // ��� ���� �� ȿ���� ���
        }
    }

    public void TakeDamage(float damage)
    {
        if (IsGuard)
        {
            // ��� ���� �� ������ ȿ���� ���
            PlayGuardSound();
            // ��� ���� �� ������ ����
        }
        else
        {
            // ��� ���� �ƴ� ���� �⺻ ������ ó�� ���� ȣ��
            playerCondition.TakeDamage(damage);
        }
    }
}
