using Photon.Pun;
using UnityEngine;
using System.Collections;

public class HealAndBoostSkill : SkillBase
{
    public float healDuration = 5f; // 힐 지속 시간
    public float totalHealAmount = 50f; // 총 힐량
    public float healRange = 5f; // 힐 범위
    public LayerMask playerLayer; // 플레이어 레이어
    public PlayerDataSO PlayerData;
    public PlayerCondition PlayerCondition;
    public AudioClip skillSound; // 스킬 효과음
    private AudioSource audioSource; // 오디오 소스

    private void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration)
        {
            return;
        }

        lastActionTime = Time.time;
        PlaySkillSound(); // 스킬 효과음 재생
        StartCoroutine(Heal());
    }

    private void PlaySkillSound()
    {
        if (skillSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(skillSound);
        }
    }

    private IEnumerator Heal()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, healRange, playerLayer); // 힐 범위

        foreach (var hitCollider in hitColliders)
        {
            PlayerCondition playerCondition = hitCollider.GetComponent<PlayerCondition>();
            if (playerCondition != null)
            {
                StartCoroutine(HealPlayer(playerCondition));
            }
        }

        yield return new WaitForSeconds(cooldownDuration);
    }

    private IEnumerator HealPlayer(PlayerCondition playerCondition)
    {
        float startTime = Time.time;
        float healPerSecond = totalHealAmount / healDuration; // 초당 힐량

        while (Time.time - startTime < healDuration)
        {
            // 힐량 계산 및 적용
            float healThisFrame = healPerSecond * Time.deltaTime;
            playerCondition.photonView.RPC(nameof(PlayerCondition.HealRPC), RpcTarget.All, healThisFrame);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
