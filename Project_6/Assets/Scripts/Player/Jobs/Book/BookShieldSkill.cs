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
    public AudioClip shieldSound; // 보호막 효과음
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    private void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;

        // 오디오 소스 컴포넌트 가져오기 또는 추가하기
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
            Debug.Log("범위 내 플레이어 없음.");
        }
    }

    private Transform FindClosestPlayer(Vector3 position, float range, LayerMask layer)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, range, layer);
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        Debug.Log($"찾은 타겟 수: {hitColliders.Length}");

        foreach (Collider2D hitCollider in hitColliders)
        {
            Debug.Log($"충돌한 객체: {hitCollider.gameObject.name}, 레이어: {hitCollider.gameObject.layer}");
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
            PlayerData.playerdefense += 50; // 방어막 적용 시 방어력 증가 (예: 50)
            Debug.Log($"{target.name}에게 보호막 적용! 방어력 증가: {PlayerData.playerdefense}");

            // 보호막 효과음 재생
            if (shieldSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shieldSound);
                Debug.Log("보호막 효과음 재생: " + shieldSound.name);
            }

            //GameObject shield = Instantiate(shieldPrefab, target.position, Quaternion.identity);
            GameObject shield = PhotonNetwork.Instantiate(shieldPrefab.name, target.position, Quaternion.identity);

            shield.transform.SetParent(target);

            // Shield 컴포넌트 추가
            if (shield.GetComponent<Book_Shield>() == null)
            {
                shield.AddComponent<Book_Shield>();
            }

            yield return new WaitForSeconds(shieldDuration);

            PlayerData.playerdefense = originalDefense; // 방어막 종료 후 방어력 원래대로 복원
            Destroy(shield);
            Debug.Log($"{target.name}의 보호막 종료! 방어력 복원: {PlayerData.playerdefense}");
        }
    }
}
