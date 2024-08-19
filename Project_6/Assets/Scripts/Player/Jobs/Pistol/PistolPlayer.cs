using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Collections;

public class PistolPlayer : RangedPlayerBase
{
    private int attackCount = 0;
    private float cooldownDuration = 2f;

    [Header("Skill Q")]
    [SerializeField] private FanningSkill fanningSkill;

    [Header("Skill E")]
    [SerializeField] private RollingSkill rollingSkill;

    [Header("Reload Sound")]
    [SerializeField] private AudioClip reloadSound; // ���� ȿ����

    private bool isUsingSkill = false;
    private bool isReloading = false; // ���� ������ Ȯ���ϴ� ����

    private void Start()
    {
        fanningSkill.SetCooldownImage(qCooldownImage);
        rollingSkill.SetCooldownImage(eCooldownImage);

        // ����� �ҽ� ������Ʈ �������� �Ǵ� �߰��ϱ�
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    public override void Attack()
    {
        if (currentAttackTime >= playerData.attackTime && !isUsingSkill && !isReloading)
        {
            base.Attack(); // �⺻ ���� ���� ȣ��

            attackCount++;

            if (attackCount >= 6)
            {
                StartReload();
            }
            else
            {
                currentAttackTime = 0f; // ��Ÿ�� �ʱ�ȭ
            }
        }
    }

    private void Update()
    {
        // �θ� Ŭ������ AttackCoolTime �޼��带 ȣ���Ͽ� ��Ÿ�� ����
        AttackCoolTime();
    }

    protected override void AttackCoolTime()
    {
        if (isReloading)
        {
            // ���� ���� �� ��Ÿ�� �ٸ� 0���� ����
            if (AttackcooldownBar != null)
            {
                AttackcooldownBar.fillAmount = 0f;
            }
        }
        else
        {
            // �⺻���� ��Ÿ�� ���� ����
            base.AttackCoolTime();
        }
    }

    public override void UseSkillQ()
    {
        if (!isUsingSkill && !isReloading)
        {
            isUsingSkill = true;
            fanningSkill.UseSkill();
            StartCoroutine(HandleFanningSkillCompletion());
        }
    }

    public override void UseSkillE()
    {
        if (!isUsingSkill && !isReloading)
        {
            rollingSkill.UseSkill();
        }
    }

    private IEnumerator HandleFanningSkillCompletion()
    {
        while (fanningSkill.IsFanningReady)
        {
            yield return null;
        }

        attackCount = 0;
        currentAttackTime = cooldownDuration; // ��ų �Ϸ� �� ��Ÿ�� �ʱ�ȭ
        isUsingSkill = false;
    }

    private void StartReload()
    {
        isReloading = true;
        PlayReloadSound(); // ���� ���� �� ���� ȿ���� ���
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(cooldownDuration); // ���� �ð� ��ٸ���
        isReloading = false;
        attackCount = 0; // ���� ī��Ʈ �ʱ�ȭ
        currentAttackTime = playerData.attackTime; // ���� ��Ÿ�� �ʱ�ȭ (��Ÿ�ӹٰ� �ٽ� ä��������)

        // ��Ÿ�� �ٰ� �ٽ� ä�������� ����
        if (AttackcooldownBar != null)
        {
            AttackcooldownBar.fillAmount = 1f;
        }
    }

    private void PlayReloadSound()
    {
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
    }
}
