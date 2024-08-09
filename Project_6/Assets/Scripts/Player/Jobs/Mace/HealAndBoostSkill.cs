using Photon.Pun;
using UnityEngine;
using System.Collections;

public class HealAndBoostSkill : SkillBase
{
    public float healDuration = 5f; // �� ���� �ð�
    public float totalHealAmount = 50f; // �� ����
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

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, healRange, playerLayer); // �� ����
        Debug.Log($"�� ���� �� ������ ������Ʈ ��: {hitColliders.Length}");
        int healedPlayers = 0;

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log($"������ ������Ʈ: {hitCollider.gameObject.name}");

            PlayerCondition playerCondition = hitCollider.GetComponent<PlayerCondition>();
            if (playerCondition != null)
            {
                Debug.Log("�÷��̾� �߰�, �� ����: " + hitCollider.gameObject.name);
                StartCoroutine(HealPlayer(playerCondition));
                healedPlayers++;
            }
        }

        if (healedPlayers > 0)
        {
            Debug.Log(healedPlayers + "���� �÷��̾ ����, ���� ���� ����");
            StartCoroutine(BoostDefense(healedPlayers));
        }

        yield return new WaitForSeconds(cooldownDuration);
        Debug.Log("Q��ų ��Ÿ�� �Ϸ�");
    }

    private IEnumerator HealPlayer(PlayerCondition playerCondition)
    {
        float startTime = Time.time;
        float healPerSecond = totalHealAmount / healDuration; // �ʴ� ����
        Debug.Log("HealPlayer �ڷ�ƾ ���۵�, �÷��̾�: " + playerCondition.name);

        while (Time.time - startTime < healDuration)
        {
            // ���� ��� �� ����
            float healThisFrame = healPerSecond * Time.deltaTime;
            playerCondition.photonView.RPC(nameof(PlayerCondition.HealRPC), RpcTarget.All, healThisFrame);
            yield return null;
        }

        Debug.Log("�� ����, �÷��̾�: " + playerCondition.name + ", ���� ü��: " + playerCondition.currentHealth);
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
