using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Collections;

public class PistolPlayer : RangedPlayerBase
{
    protected bool isAttackCooldown = false;
    protected int attackCount = 0;
    protected float cooldownDuration = 2f;

    [Header("Skill Q")]
    [SerializeField] private FanningSkill fanningSkill;

    [Header("Skill E")]
    [SerializeField] private RollingSkill rollingSkill;

    [Header("Reload Sound")]
    [SerializeField] private AudioClip reloadSound; // ���� ȿ����

    private bool isUsingSkill = false;

    private void Start()
    {
        //fanningSkill.SetCooldownText(qCooldownText);
        //rollingSkill.SetCooldownText(eCooldownText);

        fanningSkill.SetCooldownImage(qCooldownImage);
        rollingSkill.SetCooldownImage(eCooldownImage);

        // ����� �ҽ� ������Ʈ �������� �Ǵ� �߰��ϱ�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void Attack()
    {
        if (!isAttackCooldown && CanAttack() && !isUsingSkill)
        {
            base.Attack(); // �⺻ ���� ���� ȣ��

            attackCount++;

            if (attackCount >= 6)
            {
                StartCooldown();
            }
        }
    }

    private void Update()
    {
        UpdateCooldown();
    }

    private void UpdateCooldown()
    {
        if (isAttackCooldown && Time.time - lastAttackTime >= cooldownDuration)
        {
            isAttackCooldown = false;
            attackCount = 0; // ���� ī��Ʈ�� �ʱ�ȭ
        }
    }

    public override void UseSkillQ()
    {
        if (!isUsingSkill)
        {
            isUsingSkill = true;
            fanningSkill.UseSkill();
            StartCoroutine(HandleFanningSkillCompletion());
        }
    }

    public override void UseSkillE()
    {
        if (!isUsingSkill)
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
        isAttackCooldown = false;
        lastAttackTime = Time.time;
        isUsingSkill = false;
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= playerData.attackCooldown;
    }

    private void StartCooldown()
    {
        isAttackCooldown = true;
        lastAttackTime = Time.time;
        PlayReloadSound(); // ���� ���� �� ���� ȿ���� ���
    }

    private void PlayReloadSound()
    {
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
    }
}
