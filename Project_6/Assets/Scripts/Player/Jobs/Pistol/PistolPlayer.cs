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
        UpdateAttackCooldown();
    }

    private void UpdateAttackCooldown()
    {
        if (!isReloading) // ���� ���� �ƴ� ���� ��Ÿ���� ������Ʈ
        {
            if (currentAttackTime < playerData.attackTime)
            {
                currentAttackTime += Time.deltaTime;
                if (attackCooldownbar != null)
                {
                    attackCooldownbar.fillAmount = currentAttackTime / playerData.attackTime;
                }
            }
        }
        else
        {
            // ���� �߿��� ���� �ٰ� ���� �ʵ��� ����
            if (attackCooldownbar != null)
            {
                attackCooldownbar.fillAmount = 0f;
            }
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
        currentAttackTime = 0f; // ���� ��Ÿ�� �ʱ�ȭ
    }

    private void PlayReloadSound()
    {
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
    }
}
