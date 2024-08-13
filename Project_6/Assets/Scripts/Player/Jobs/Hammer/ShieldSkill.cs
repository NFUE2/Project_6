using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ShieldSkill : SkillBase
{
    public float ShieldDuration; // 방패 지속 시간
    public GameObject shieldPrefab; // 방패 프리팹
    public float shieldDistance = 1f; // 캐릭터 앞 방패 거리
    private GameObject createdShield; // 생성된 방패를 저장할 변수
    public PlayerDataSO PlayerData; // 플레이어 데이터를 저장할 ScriptableObject
    public AudioClip shieldSound; // 방어 시 효과음 추가
    private AudioSource audioSource; // AudioSource 컴포넌트 추가

    private Coroutine followCoroutine;
    private Coroutine destroyCoroutine;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown; // 쿨다운 시간을 플레이어 데이터에서 가져옴
        lastActionTime = -cooldownDuration; // lastActionTime을 초기화하여 처음에 쿨다운이 적용되지 않도록 함
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
    }

    public override void UseSkill()
    {
        if (createdShield != null || !IsSkillReady()) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z값을 0으로 설정하여 2D 공간에서 사용

        // 플레이어와 마우스 사이의 방향 계산
        Vector3 direction = (mousePosition - transform.position).normalized;

        // 방패 생성 (부모-자식 관계 없이 독립적인 오브젝트로 생성)
        createdShield = PhotonNetwork.Instantiate(shieldPrefab.name, transform.position, Quaternion.identity);

        // 방패의 박스 콜라이더 중심을 기준으로 방패 위치 설정
        if (createdShield.TryGetComponent<BoxCollider2D>(out BoxCollider2D shieldCollider))
        {
            Vector3 shieldCenter = shieldCollider.bounds.center; // 박스 콜라이더의 중심
            Vector3 offset = direction * shieldDistance;

            // 방패 위치를 콜라이더 중심에 맞게 설정
            createdShield.transform.position = transform.position + offset - (shieldCenter - createdShield.transform.position);
        }

        // 방패의 회전 설정 (마우스 방향으로 회전)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        createdShield.transform.rotation = Quaternion.Euler(0, 5, angle);

        // 방패가 플레이어를 따라 이동하도록 코루틴 시작
        if (followCoroutine != null) StopCoroutine(followCoroutine);
        followCoroutine = StartCoroutine(FollowPlayer(direction));

        PlayShieldSound();

        lastActionTime = Time.time;

        if (destroyCoroutine != null) StopCoroutine(destroyCoroutine);
        destroyCoroutine = StartCoroutine(DestroyShieldAfterTime());
    }

    private IEnumerator FollowPlayer(Vector3 initialDirection)
    {
        while (createdShield != null)
        {
            // 플레이어의 현재 위치를 기준으로 방패의 위치를 업데이트
            Vector3 newPosition = transform.position + initialDirection * shieldDistance;
            createdShield.transform.position = newPosition;

            yield return null; // 매 프레임마다 업데이트
        }
    }

    private bool IsSkillReady()
    {
        return Time.time - lastActionTime >= cooldownDuration;
    }

    private IEnumerator DestroyShieldAfterTime()
    {
        yield return new WaitForSeconds(ShieldDuration);
        if (createdShield != null)
        {
            PhotonNetwork.Destroy(createdShield);
        }
    }

    private void PlayShieldSound()
    {
        if (shieldSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shieldSound);
        }
    }
}
