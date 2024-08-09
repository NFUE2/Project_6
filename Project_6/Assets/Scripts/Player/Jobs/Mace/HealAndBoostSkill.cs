using Photon.Pun;
using UnityEngine;
using System.Collections;

public class HealAndBoostSkill : SkillBase
{
    public float healDuration = 5f; // 힐 지속 시간
    public float totalHealAmount = 50f; // 총 힐량
    public float statBoostDuration = 10f; // 스탯 강화 지속 시간
    public float defenseBoost = 10f; // 방어력 증가량
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
        Debug.Log("HealAndBoostSkill 시작됨, 쿨타임 설정: " + cooldownDuration);
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("스킬 쿨타임이 아직 끝나지 않음");
            return;
        }

        lastActionTime = Time.time;
        Debug.Log("스킬 사용됨, 현재 시간: " + Time.time);
        PlaySkillSound(); // 스킬 효과음 재생
        StartCoroutine(HealAndBoost());
    }

    private void PlaySkillSound()
    {
        if (skillSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(skillSound);
        }
    }

    private IEnumerator HealAndBoost()
    {
        Debug.Log("HealAndBoost 코루틴 시작됨");

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, healRange, playerLayer); // 힐 범위
        Debug.Log($"힐 범위 내 감지된 오브젝트 수: {hitColliders.Length}");
        int healedPlayers = 0;

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log($"감지된 오브젝트: {hitCollider.gameObject.name}");

            PlayerCondition playerCondition = hitCollider.GetComponent<PlayerCondition>();
            if (playerCondition != null)
            {
                Debug.Log("플레이어 발견, 힐 시작: " + hitCollider.gameObject.name);
                StartCoroutine(HealPlayer(playerCondition));
                healedPlayers++;
            }
        }

        if (healedPlayers > 0)
        {
            Debug.Log(healedPlayers + "명의 플레이어가 힐됨, 방어력 증가 시작");
            StartCoroutine(BoostDefense(healedPlayers));
        }

        yield return new WaitForSeconds(cooldownDuration);
        Debug.Log("Q스킬 쿨타임 완료");
    }

    private IEnumerator HealPlayer(PlayerCondition playerCondition)
    {
        float startTime = Time.time;
        float healPerSecond = totalHealAmount / healDuration; // 초당 힐량
        Debug.Log("HealPlayer 코루틴 시작됨, 플레이어: " + playerCondition.name);

        while (Time.time - startTime < healDuration)
        {
            // 힐량 계산 및 적용
            float healThisFrame = healPerSecond * Time.deltaTime;
            playerCondition.photonView.RPC(nameof(PlayerCondition.HealRPC), RpcTarget.All, healThisFrame);
            yield return null;
        }

        Debug.Log("힐 종료, 플레이어: " + playerCondition.name + ", 최종 체력: " + playerCondition.currentHealth);
    }

    private IEnumerator BoostDefense(int healedPlayers)
    {
        float originalDefense = PlayerData.playerdefense; // 현재 방어력 저장
        Debug.Log("BoostDefense 코루틴 시작됨, 원래 방어력: " + originalDefense);

        PlayerData.playerdefense += defenseBoost * healedPlayers;
        Debug.Log("방어력 증가, 증가량: " + (defenseBoost * healedPlayers));

        yield return new WaitForSeconds(statBoostDuration);

        PlayerData.playerdefense = originalDefense; // 방어력 원상복구
        Debug.Log("방어력 원상복구, 현재 방어력: " + PlayerData.playerdefense);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
