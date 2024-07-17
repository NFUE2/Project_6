using UnityEngine;
using TMPro;

public class GuardSkill : SkillBase
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 1.0f; // ���� ���� �ð�
    public PlayerDataSO PlayerData;

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
            IsGuard = true;
            // animator.SetBool("Guard", true);
            Invoke("ExitGuardEvent", GuardDuration); // ���� �ð� �̺�Ʈ ����
        }
    }

    private void ExitGuard()
    {
        Debug.Log("���� ����");
        IsGuard = false;
        // animator.SetBool("Guard", false);
        lastActionTime = Time.time;
    }

    private void ExitGuardEvent()
    {
        if (IsGuard) ExitGuard();
    }
}
