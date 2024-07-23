using UnityEngine;
using TMPro;

public class GuardSkill : SkillBase
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 1.0f; // ���� ���� �ð�
    public float damageReductionDuringGuard = 0.5f; // ���� �� �޴� ������ ���� ����
    public PlayerDataSO PlayerData;
    public float damageReduction;


    private float originalDamageReduction;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
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
        originalDamageReduction = damageReduction; // ���� ������ ���� ���� ����
        damageReduction *= damageReductionDuringGuard; // ������ ���� ����
        // animator.SetBool("Guard", true);
        Invoke("ExitGuardEvent", GuardDuration); // ���� �ð� �̺�Ʈ ����
    }

    private void ExitGuard()
    {
        Debug.Log("���� ����");
        IsGuard = false;
        damageReduction = originalDamageReduction; // ������ ���� ���� ����
        // animator.SetBool("Guard", false);
        lastActionTime = Time.time;
    }

    private void ExitGuardEvent()
    {
        if (IsGuard) ExitGuard();
    }
}
