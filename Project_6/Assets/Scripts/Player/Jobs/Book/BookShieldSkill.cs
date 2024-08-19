using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;

public class BookShieldSkill : SkillBase
{
    public float shieldDuration = 5f;
    public float shieldRange = 10f;
    public GameObject shieldPrefab;
    public GameObject shieldEffectPrefab; // �� ��ƼŬ ȿ�� ������ �߰�
    public LayerMask playerLayer;
    public PlayerDataSO PlayerData;
    public AudioClip shieldSound; // ��ȣ�� ȿ����
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    private void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;

        // ����� �ҽ� ������Ʈ �������� �Ǵ� �߰��ϱ�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        lastActionTime = Time.time;

        // �÷��̾ 1���� ��� �ڱ� �ڽſ��� ���� ����
        if (GameManager.Instance.players.Count == 1)
        {
            StartCoroutine(ApplyShield(transform, GameManager.Instance.players.IndexOf(this.gameObject)));
        }
        else
        {
            // ���� ����� �÷��̾��� �ε����� ã��
            int closestPlayerIndex = FindClosestPlayerIndex(transform.position, shieldRange);

            if (closestPlayerIndex != -1)
            {
                // �ε����� ����Ͽ� ���� ����� �÷��̾��� Transform�� ������
                Transform closestPlayer = GameManager.Instance.players[closestPlayerIndex].transform;
                StartCoroutine(ApplyShield(closestPlayer, closestPlayerIndex));
            }
            else
            {
                // ����� �÷��̾ ������ ���� ������� ����
                Debug.Log("���� ����� �÷��̾ �����ϴ�.");
            }
        }
    }

    // ���� ����� �÷��̾��� �ε����� ��ȯ�ϴ� �Լ�
    private int FindClosestPlayerIndex(Vector3 position, float range)
    {
        List<GameObject> players = GameManager.Instance.players;
        int closestIndex = -1;
        float closestDistance = Mathf.Infinity;

        // ��� �÷��̾ ��ȸ�ϸ� ���� ����� �÷��̾ ã��
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == this.gameObject) continue; // �ڱ� �ڽ��� ����

            float distance = Vector3.Distance(position, players[i].transform.position);
            if (distance < closestDistance && distance <= range)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // ��ȣ���� �����ϴ� �ڷ�ƾ
    private IEnumerator ApplyShield(Transform target, int index)
    {
        PlayerBase player = target.GetComponent<PlayerBase>();

        if (player != null)
        {
            float originalDefense = PlayerData.defence;
            PlayerData.defence += 50; // �� ���� �� ���� ���� (��: 50)

            // ��ȣ�� ȿ���� ���
            if (shieldSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shieldSound);
            }

            // ��ȣ�� �������� �����ϰ� Ÿ�ٿ� ����
            GameObject shield = PhotonNetwork.Instantiate(shieldPrefab.name, target.position, Quaternion.identity);
            if (shield.TryGetComponent(out Book_Shield b)) b.SetParent(index);

            // ��ƼŬ ȿ�� ���� �� Ÿ�ٿ� ����
            if (shieldEffectPrefab != null)
            {
                Vector3 effectPosition = target.position;
                effectPosition.y += 0.4f; // ��ƼŬ ȿ���� Ÿ�� ��ġ���� 1.0f ���� ����

                GameObject effect = Instantiate(shieldEffectPrefab, effectPosition, Quaternion.identity);
                effect.transform.SetParent(target); // ��ƼŬ�� Ÿ�ٿ� ����
                Destroy(effect, shieldDuration); // ��ƼŬ�� �� ���� �ð� �Ŀ� ����
            }

            // ������ ���� �ð� ���� ��ȣ�� ����
            yield return new WaitForSeconds(shieldDuration);

            // ��ȣ�� ���� �� ���� ������� ����
            PlayerData.defence = originalDefense;
            PhotonNetwork.Destroy(shield);
        }
    }
}
