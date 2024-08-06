using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;

public class BookShieldSkill : SkillBase
{
    public float shieldDuration = 5f;
    public float shieldRange = 10f;
    public GameObject shieldPrefab;
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

        // ���� ����� �÷��̾��� �ε����� ã��
        int closestPlayerIndex = FindClosestPlayerIndex(transform.position, shieldRange);

        if (closestPlayerIndex != -1)
        {
            // �ε����� ����Ͽ� ���� ����� �÷��̾��� Transform�� ������
            Transform closestPlayer = GameManager.Instance.players[closestPlayerIndex].transform;
            StartCoroutine(ApplyShield(closestPlayer,closestPlayerIndex));
        }
        else
        {
            Debug.Log("���� �� �÷��̾� ����.");
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
    private IEnumerator ApplyShield(Transform target,int index)
    {
        PlayerBase player = target.GetComponent<PlayerBase>();

        if (player != null)
        {
            float originalDefense = PlayerData.playerdefense;
            PlayerData.playerdefense += 50; // �� ���� �� ���� ���� (��: 50)
            //Debug.Log($"{target.name}���� ��ȣ�� ����! ���� ����: {PlayerData.playerdefense}");

            // ��ȣ�� ȿ���� ���
            if (shieldSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shieldSound);
                Debug.Log("��ȣ�� ȿ���� ���: " + shieldSound.name);
            }

            // ��ȣ�� �������� �����ϰ� Ÿ�ٿ� ����
            GameObject shield = PhotonNetwork.Instantiate("Prefabs/" + shieldPrefab.name, target.position, Quaternion.identity);
            if (shield.TryGetComponent(out Book_Shield b)) b.SetParent(index);
            //shield.transform.SetParent(target);

            // Shield ������Ʈ �߰� (�ʿ��� ���)
            //if (shield.GetComponent<Book_Shield>() == null)
            //{
            //    shield.AddComponent<Book_Shield>();
            //}

            // ������ ���� �ð� ���� ��ȣ�� ����
            yield return new WaitForSeconds(shieldDuration);

            // ��ȣ�� ���� �� ���� ������� ����
            PlayerData.playerdefense = originalDefense;
            //Destroy(shield);
            PhotonNetwork.Destroy(shield);
            Debug.Log($"{target.name}�� ��ȣ�� ����! ���� ����: {PlayerData.playerdefense}");
        }
    }
}
