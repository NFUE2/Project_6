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
        fanningSkill.SetCooldownText(qCooldownText);
        rollingSkill.SetCooldownText(eCooldownText);

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
            Debug.Log($"Attack {attackCount}: Performed an attack.");

            if (attackCount >= 6)
            {
                StartCooldown();
            }
        }
        else
        {
            Debug.Log("Attack is on cooldown or cannot attack yet.");
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
            Debug.Log("Cooldown complete: You can attack again.");
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
        Debug.Log("Fanning skill complete: Fully reloaded.");
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
        Debug.Log("Reloading: Attack count reached 6, starting cooldown.");
    }

    private void PlayReloadSound()
    {
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
            Debug.Log("Reload sound played: " + reloadSound.name);
        }
        else
        {
            Debug.LogError("reloadSound �Ǵ� audioSource�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
