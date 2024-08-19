using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;

public class BookShieldSkill : SkillBase
{
    public float shieldDuration = 5f;
    public float shieldRange = 10f;
    public GameObject shieldPrefab;
    public GameObject shieldEffectPrefab; // 방어막 파티클 효과 프리팹 추가
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

        // 플레이어가 1명일 경우 자기 자신에게 방어막을 적용
        if (GameManager.Instance.players.Count == 1)
        {
            StartCoroutine(ApplyShield(transform, GameManager.Instance.players.IndexOf(this.gameObject)));
        }
        else
        {
            // 가장 가까운 플레이어의 인덱스를 찾음
            int closestPlayerIndex = FindClosestPlayerIndex(transform.position, shieldRange);

            if (closestPlayerIndex != -1)
            {
                // 인덱스를 사용하여 가장 가까운 플레이어의 Transform을 가져옴
                Transform closestPlayer = GameManager.Instance.players[closestPlayerIndex].transform;
                StartCoroutine(ApplyShield(closestPlayer, closestPlayerIndex));
            }
            else
            {
                // 가까운 플레이어가 없으면 방어막을 사용하지 않음
                Debug.Log("방어막을 사용할 플레이어가 없습니다.");
            }
        }
    }

    // 가장 가까운 플레이어의 인덱스를 반환하는 함수
    private int FindClosestPlayerIndex(Vector3 position, float range)
    {
        List<GameObject> players = GameManager.Instance.players;
        int closestIndex = -1;
        float closestDistance = Mathf.Infinity;

        // 모든 플레이어를 순회하며 가장 가까운 플레이어를 찾음
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == this.gameObject) continue; // 자기 자신은 무시

            float distance = Vector3.Distance(position, players[i].transform.position);
            if (distance < closestDistance && distance <= range)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // 보호막을 적용하는 코루틴
    private IEnumerator ApplyShield(Transform target, int index)
    {
        PlayerBase player = target.GetComponent<PlayerBase>();

        if (player != null)
        {
            float originalDefense = PlayerData.defence;
            PlayerData.defence += 50; // 방어막 적용 시 방어력 증가 (예: 50)

            // 보호막 효과음 재생
            if (shieldSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shieldSound);
            }

            // 보호막 프리팹을 생성하고 타겟에 부착
            GameObject shield = PhotonNetwork.Instantiate(shieldPrefab.name, target.position, Quaternion.identity);
            if (shield.TryGetComponent(out Book_Shield b)) b.SetParent(index);

            // 파티클 효과 생성 및 타겟에 부착
            if (shieldEffectPrefab != null)
            {
                Vector3 effectPosition = target.position;
                effectPosition.y += 0.4f; // 파티클 효과를 타겟 위치보다 1.0f 위에 생성

                GameObject effect = Instantiate(shieldEffectPrefab, effectPosition, Quaternion.identity);
                effect.transform.SetParent(target); // 파티클을 타겟에 부착
                Destroy(effect, shieldDuration); // 파티클을 방어막 지속 시간 후에 제거
            }

            // 지정된 지속 시간 동안 보호막 유지
            yield return new WaitForSeconds(shieldDuration);

            // 보호막 종료 후 방어력 원래대로 복원
            PlayerData.defence = originalDefense;
            PhotonNetwork.Destroy(shield);
        }
    }
}
