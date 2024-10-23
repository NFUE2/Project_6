using Photon.Pun;
using UnityEngine;
using System.Collections;

public class HealAndBoostSkill : SkillBase
{
    public float healDuration = 5f; // �� ���� �ð�
    public float totalHealAmount = 50f; // �� ����
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
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration)
        {
            return;
        }

        lastActionTime = Time.time;
        PlaySkillSound(); // ��ų ȿ���� ���
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, healRange, playerLayer); // �� ����

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
        float healPerSecond = totalHealAmount / healDuration; // �ʴ� ����

        while (Time.time - startTime < healDuration)
        {
            // ���� ��� �� ����
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
