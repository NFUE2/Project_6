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
    [SerializeField] private AudioClip reloadSound; // 장전 효과음

    private void Start()
    {
        fanningSkill.SetCooldownText(qCooldownText);
        rollingSkill.SetCooldownText(eCooldownText);

        // 오디오 소스 컴포넌트 가져오기 또는 추가하기
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
            // RangedPlayerBase의 Attack 메서드를 호출하여 공격을 수행
            base.Attack();
            attackCount++;
            Debug.Log($"Attack {attackCount}: Performed an attack.");

            if (attackCount >= 6)
            {
                isAttackCooldown = true;
                attackCount = 0;
                lastAttackTime = Time.time;
                Debug.Log("Reloading: Attack count reached 6, starting cooldown.");
                PlayReloadSound(); // 장전 시작 시 장전 효과음 재생
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
            Debug.LogError("reloadSound 또는 audioSource가 할당되지 않았습니다.");
        }
    }
}
