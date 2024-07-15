using System.Collections;
using TMPro;
using UnityEngine;

public class GuardSkill : MonoBehaviour
{
    public bool IsGuard { get; private set; }
    public float GuardDuration = 1.0f; // ���� ���� �ð�

    //����Ŭ�������� ó��
    public PlayerData PlayerData;
    private float lastActionTime;
    private TextMeshProUGUI cooldownText;
    //====================================

    //��Ÿ���� ����Ŭ�������� ó��
    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }

    //�������̵�, �÷��̾ �������϶� ������ �־���ϴµ� ó���ʿ�
    public void UseSkill()
    {
        if (IsGuard)
        {
            ExitGuard();
        }
        else
        {
            if (Time.time - lastActionTime < PlayerData.SkillQCooldown) return; // Q ��ų ��Ÿ�� üũ

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

    //�������̽� Ŭ������ ó��
    private void ExitGuardEvent()
    {
        if (IsGuard) ExitGuard();
    }


    //��Ÿ�� ����Ŭ�������� ó��
    private void Update()
    {
        if (cooldownText != null)
        {
            if (Time.time - lastActionTime >= PlayerData.SkillQCooldown)
            {
                cooldownText.text = "Q��ų ��Ÿ�� �Ϸ�"; // ��Ÿ�� �Ϸ� �ؽ�Ʈ ����
            }
            else
            {
                float remainingTime = PlayerData.SkillQCooldown - (Time.time - lastActionTime);
                cooldownText.text = $"{remainingTime:F1}"; // ��Ÿ�� �ؽ�Ʈ ����
            }
        }
    }
}