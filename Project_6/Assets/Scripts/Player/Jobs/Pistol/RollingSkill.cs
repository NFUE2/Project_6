using UnityEngine;
using System.Collections;

public class RollingSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    [SerializeField] private float rollingDistance = 5f; // 구르는 거리
    [SerializeField] private float rollingDuration = 0.5f; // 구르는 지속 시간
    [SerializeField] private AudioClip rollingSound; // 롤링 효과음

    private bool isRolling = false; // 롤링 중인지 여부
    private bool isInvincible = false; // 무적 상태인지 여부
    private AudioSource audioSource;
    private Vector3 targetPosition;

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;

        // 오디오 소스 컴포넌트 가져오기 또는 추가하기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        if (isRolling || Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("RollingSkill: Cannot use skill, either on cooldown or already rolling.");
            return;
        }

        Debug.Log("RollingSkill: Skill used.");
        StartCoroutine(Rolling());
    }

    private IEnumerator Rolling()
    {
        isRolling = true;
        isInvincible = true;
        lastActionTime = Time.time;

        // 애니메이션 시작 (애니메이션 코드 추가 예정)
        // GetComponent<Animator>().SetTrigger("Roll");

        // 이동 방향 및 목표 위치 계산
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z축 값 무시
        Vector3 direction = (mousePosition - transform.position).normalized;
        targetPosition = transform.position + direction * rollingDistance;

        // 롤링 효과음 재생
        PlayRollingSound();

        // Lerp를 사용하여 부드럽게 이동
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time < startTime + rollingDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (Time.time - startTime) / rollingDuration);
            yield return null;
        }

        transform.position = targetPosition; // 최종 위치 설정
        Debug.Log("RollingSkill: Rolling ended.");

        isRolling = false;
        isInvincible = false;

        // 애니메이션 종료 (애니메이션 코드 추가 예정)
        // GetComponent<Animator>().SetTrigger("StopRoll");
    }

    private void PlayRollingSound()
    {
        if (rollingSound != null)
        {
            audioSource.PlayOneShot(rollingSound);
        }
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
