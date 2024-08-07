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
    [SerializeField] private AudioClip reloadSound; // 장전 효과음

    private bool isUsingSkill = false;

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
        if (!isAttackCooldown && CanAttack() && !isUsingSkill)
        {
            base.Attack(); // 기본 공격 로직 호출

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
            attackCount = 0; // 공격 카운트를 초기화
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
        PlayReloadSound(); // 장전 시작 시 장전 효과음 재생
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
            Debug.LogError("reloadSound 또는 audioSource가 할당되지 않았습니다.");
        }
    }
}
