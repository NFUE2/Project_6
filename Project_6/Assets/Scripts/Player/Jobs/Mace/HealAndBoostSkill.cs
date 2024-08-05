using UnityEngine;
using System.Collections;

public class HealAndBoostSkill : SkillBase
{
    public float healDuration = 5f; // 힐 지속 시간
    public float healAmount = 10f; // 힐량
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

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, healRange); // 힐 범위
        int healedPlayers = 0;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                MacePlayer player = hitCollider.GetComponent<MacePlayer>();
                PlayerCondition playerCondition = hitCollider.GetComponent<PlayerCondition>();
                if (player != null && playerCondition != null)
                {
                    Debug.Log("플레이어 발견, 힐 시작: " + player.name);
                    StartCoroutine(HealPlayer(playerCondition));
                    healedPlayers++;
                }
            }
        }

        if (healedPlayers > 0)
        {
            Debug.Log(healedPlayers + "명의 플레이어가 힐됨, 방어력 증가 시작");
            StartCoroutine(BoostDefense(healedPlayers));
        }

        yield return new WaitForSeconds(PlayerData.SkillQCooldown);
        Debug.Log("Q스킬 쿨타임 완료");
    }

    private IEnumerator HealPlayer(PlayerCondition playerCondition)
    {
        float startTime = Time.time;
        Debug.Log("HealPlayer 코루틴 시작됨, 플레이어: " + playerCondition.name);

        while (Time.time - startTime < healDuration)
        {
            // 힐량 계산 및 적용
            playerCondition.currentHealth += healAmount * Time.deltaTime;
            playerCondition.currentHealth = Mathf.Clamp(playerCondition.currentHealth, 0, playerCondition.maxHealth); // 체력 범위 제한
            Debug.Log($"힐량: {healAmount * Time.deltaTime}, 현재 체력: {playerCondition.currentHealth}");
            yield return null;
        }

        Debug.Log("힐 종료, 플레이어: " + playerCondition.name);
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
