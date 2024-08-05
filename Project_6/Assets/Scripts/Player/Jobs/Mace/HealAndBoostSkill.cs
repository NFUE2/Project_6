using UnityEngine;
using System.Collections;

public class HealAndBoostSkill : SkillBase
{
    public float healDuration = 5f; // �� ���� �ð�
    public float healAmount = 10f; // ����
    public float statBoostDuration = 10f; // ���� ��ȭ ���� �ð�
    public float defenseBoost = 10f; // ���� ������
    public float healRange = 5f; // �� ����
    public LayerMask playerLayer; // �÷��̾� ���̾�
    public PlayerDataSO PlayerData;
    public PlayerCondition PlayerCondition;
    public AudioClip skillSound; // ��ų ȿ����
    private AudioSource audioSource; // ����� �ҽ�

    private void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Debug.Log("HealAndBoostSkill ���۵�, ��Ÿ�� ����: " + cooldownDuration);
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("��ų ��Ÿ���� ���� ������ ����");
            return;
        }

        lastActionTime = Time.time;
        Debug.Log("��ų ����, ���� �ð�: " + Time.time);
        PlaySkillSound(); // ��ų ȿ���� ���
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
        Debug.Log("HealAndBoost �ڷ�ƾ ���۵�");

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, healRange); // �� ����
        int healedPlayers = 0;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                MacePlayer player = hitCollider.GetComponent<MacePlayer>();
                PlayerCondition playerCondition = hitCollider.GetComponent<PlayerCondition>();
                if (player != null && playerCondition != null)
                {
                    Debug.Log("�÷��̾� �߰�, �� ����: " + player.name);
                    StartCoroutine(HealPlayer(playerCondition));
                    healedPlayers++;
                }
            }
        }

        if (healedPlayers > 0)
        {
            Debug.Log(healedPlayers + "���� �÷��̾ ����, ���� ���� ����");
            StartCoroutine(BoostDefense(healedPlayers));
        }

        yield return new WaitForSeconds(PlayerData.SkillQCooldown);
        Debug.Log("Q��ų ��Ÿ�� �Ϸ�");
    }

    private IEnumerator HealPlayer(PlayerCondition playerCondition)
    {
        float startTime = Time.time;
        Debug.Log("HealPlayer �ڷ�ƾ ���۵�, �÷��̾�: " + playerCondition.name);

        while (Time.time - startTime < healDuration)
        {
            // ���� ��� �� ����
            playerCondition.currentHealth += healAmount * Time.deltaTime;
            playerCondition.currentHealth = Mathf.Clamp(playerCondition.currentHealth, 0, playerCondition.maxHealth); // ü�� ���� ����
            Debug.Log($"����: {healAmount * Time.deltaTime}, ���� ü��: {playerCondition.currentHealth}");
            yield return null;
        }

        Debug.Log("�� ����, �÷��̾�: " + playerCondition.name);
    }

    private IEnumerator BoostDefense(int healedPlayers)
    {
        float originalDefense = PlayerData.playerdefense; // ���� ���� ����
        Debug.Log("BoostDefense �ڷ�ƾ ���۵�, ���� ����: " + originalDefense);

        PlayerData.playerdefense += defenseBoost * healedPlayers;
        Debug.Log("���� ����, ������: " + (defenseBoost * healedPlayers));

        yield return new WaitForSeconds(statBoostDuration);

        PlayerData.playerdefense = originalDefense; // ���� ���󺹱�
        Debug.Log("���� ���󺹱�, ���� ����: " + PlayerData.playerdefense);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
