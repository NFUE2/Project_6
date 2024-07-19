using UnityEngine;
using System.Collections;

public class HealAndBoostSkill : SkillBase
{
    public float healDuration = 5f; // �� ���� �ð�
    public float healAmount = 10f; // ����
    public float statBoostDuration = 10f; // ���� ��ȭ ���� �ð�
    public float defenseBoost = 10f; // ���� ������
    public LayerMask playerLayer; // �÷��̾� ���̾�
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // �� ����

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
        Debug.Log("Q��ų ��Ÿ�� �Ϸ�");
    }

    private IEnumerator HealPlayer(MacePlayer player)
    {
        float startTime = Time.time;
        while (Time.time - startTime < healDuration)
        {
            //player.health += healAmount * Time.deltaTime;
            //player.health = Mathf.Clamp(player.health, 0, 100); // ü�� ���� ����
            //Debug.Log($"����: {healAmount * Time.deltaTime}, ���� ü��: {player.health}");
            yield return null;
        }
    }

    private IEnumerator BoostDefense(int healedPlayers)
    {
        float originalDefense = PlayerData.playerdefense; // ���� ���� ����
        PlayerData.playerdefense += defenseBoost * healedPlayers;
        yield return new WaitForSeconds(statBoostDuration);
        PlayerData.playerdefense = originalDefense; // ���� ���󺹱�
    }
}
