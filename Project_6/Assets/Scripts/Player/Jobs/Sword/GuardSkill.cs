using UnityEngine;
using TMPro;

public class GuardSkill : SkillBase, IDamagable
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 1.0f; // ���� ���� �ð�
    public float DefenseBoostDuringGuard = 50f; // ���� �� ���� ������
    public PlayerDataSO PlayerData;
    public float DamageReduction;

    private Animator animator;
    private PlayerCondition playerCondition;
    private float originalDamageReduction;

    public AudioClip guardSound; // ��� ���� �� ȿ���� �߰�
    private AudioSource audioSource; // AudioSource ������Ʈ �߰�

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
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

            Debug.Log("Q ��ų ���");
            EnterGuard();
        }
    }

    private void EnterGuard()
    {
        IsGuard = true;
        SaveOriginalStats();
        ApplyGuardStats();
        animator.SetBool("IsGuard", true); // ���� �ִϸ��̼� ����
        Invoke("ExitGuardEvent", GuardDuration); // ���� �ð� �̺�Ʈ ����
    }

    private void ExitGuard()
    {
        Debug.Log("���� ����");
        IsGuard = false;
        RestoreOriginalStats();
        animator.SetBool("IsGuard", false); // ���� �ִϸ��̼� ����
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
            // �������� ������ �� ������, ���⼭ ������ ó�� ������ �߰��� �� ����
        }
        else
        {
            // ��� ���� �ƴ� ���� �⺻ ������ ó�� ������ ȣ���ϰų� ó���� �� ����
            playerCondition.TakeDamage(damage); // �� �κ��� �������̽� ������ ���� �ٸ�
        }
    }
}
