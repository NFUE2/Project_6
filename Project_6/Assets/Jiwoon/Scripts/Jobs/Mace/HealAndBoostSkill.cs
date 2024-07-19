using UnityEngine;
using System.Collections;

public class HealAndBoostSkill : SkillBase
{
    public float healDuration = 5f; // 힐 지속 시간
    public float healAmount = 10f; // 힐량
    public float statBoostDuration = 10f; // 스탯 강화 지속 시간
    public float defenseBoost = 10f; // 방어력 증가량
    public LayerMask playerLayer; // 플레이어 레이어
    public PlayerDataSO PlayerData;

    private void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        lastActionTime = Time.time;
        StartCoroutine(HealAndBoost());
    }

    private IEnumerator HealAndBoost()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // 힐 범위

        int healedPlayers = 0;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                MacePlayer player = hitCollider.GetComponent<MacePlayer>();
                if (player != null && player != this)
                {
                    StartCoroutine(HealPlayer(player));
                    healedPlayers++;
                }
            }
        }

        if (healedPlayers > 0)
        {
            StartCoroutine(BoostDefense(healedPlayers));
        }

        yield return new WaitForSeconds(PlayerData.SkillQCooldown);
        Debug.Log("Q스킬 쿨타임 완료");
    }

    private IEnumerator HealPlayer(MacePlayer player)
    {
        float startTime = Time.time;
        while (Time.time - startTime < healDuration)
        {
            //player.health += healAmount * Time.deltaTime;
            //player.health = Mathf.Clamp(player.health, 0, 100); // 체력 범위 제한
            //Debug.Log($"힐량: {healAmount * Time.deltaTime}, 현재 체력: {player.health}");
            yield return null;
        }
    }

    private IEnumerator BoostDefense(int healedPlayers)
    {
        float originalDefense = PlayerData.playerdefense; // 현재 방어력 저장
        PlayerData.playerdefense += defenseBoost * healedPlayers;
        yield return new WaitForSeconds(statBoostDuration);
        PlayerData.playerdefense = originalDefense; // 방어력 원상복구
    }
}
