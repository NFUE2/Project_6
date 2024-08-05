using UnityEngine;
using System.Collections;
using Photon.Pun;

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
        Transform closestPlayer = FindClosestPlayer(transform.position, shieldRange, playerLayer);

        if (closestPlayer != null)
        {
            StartCoroutine(ApplyShield(closestPlayer));
        }
        else
        {
            Debug.Log("���� �� �÷��̾� ����.");
        }
    }

    private Transform FindClosestPlayer(Vector3 position, float range, LayerMask layer)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, range, layer);
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        Debug.Log($"ã�� Ÿ�� ��: {hitColliders.Length}");

        foreach (Collider2D hitCollider in hitColliders)
        {
            Debug.Log($"�浹�� ��ü: {hitCollider.gameObject.name}, ���̾�: {hitCollider.gameObject.layer}");
            if (hitCollider.transform == transform) continue;

            float distance = Vector3.Distance(position, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = hitCollider.transform;
            }
        }

        return closestPlayer;
    }


    private IEnumerator ApplyShield(Transform target)
    {
        PlayerBase player = target.GetComponent<PlayerBase>();

        if (player != null)
        {
            float originalDefense = PlayerData.playerdefense;
            PlayerData.playerdefense += 50; // �� ���� �� ���� ���� (��: 50)
            Debug.Log($"{target.name}���� ��ȣ�� ����! ���� ����: {PlayerData.playerdefense}");

            // ��ȣ�� ȿ���� ���
            if (shieldSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shieldSound);
                Debug.Log("��ȣ�� ȿ���� ���: " + shieldSound.name);
            }

            //GameObject shield = Instantiate(shieldPrefab, target.position, Quaternion.identity);
            GameObject shield = PhotonNetwork.Instantiate(shieldPrefab.name, target.position, Quaternion.identity);

            shield.transform.SetParent(target);

            // Shield ������Ʈ �߰�
            if (shield.GetComponent<Book_Shield>() == null)
            {
                shield.AddComponent<Book_Shield>();
            }

            yield return new WaitForSeconds(shieldDuration);

            PlayerData.playerdefense = originalDefense; // �� ���� �� ���� ������� ����
            Destroy(shield);
            Debug.Log($"{target.name}�� ��ȣ�� ����! ���� ����: {PlayerData.playerdefense}");
        }
    }
}
