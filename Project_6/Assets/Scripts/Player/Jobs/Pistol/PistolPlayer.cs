using Photon.Pun;
using UnityEngine;
using TMPro;

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
        if (!isAttackCooldown && CanAttack())
        {
            // RangedPlayerBase�� Attack �޼��带 ȣ���Ͽ� ������ ����
            base.Attack();
            attackCount++;
            Debug.Log($"Attack {attackCount}: Performed an attack.");

            if (attackCount >= 6)
            {
                isAttackCooldown = true;
                attackCount = 0;
                lastAttackTime = Time.time;
                Debug.Log("Reloading: Attack count reached 6, starting cooldown.");
                PlayReloadSound(); // ���� ���� �� ���� ȿ���� ���
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
            Debug.Log("Cooldown complete: You can attack again.");
        }
    }

    public override void UseSkillQ()
    {
        fanningSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        rollingSkill.UseSkill();
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= playerData.attackCooldown;
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
