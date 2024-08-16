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
    private Rigidbody2D rb2d; // Rigidbody2D 컴포넌트

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;

        // 오디오 소스 컴포넌트 가져오기 또는 추가하기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Rigidbody2D 컴포넌트 가져오기
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    public override void UseSkill()
    {
        if (isRolling || Time.time - lastActionTime < cooldownDuration)
        {
            return;
        }
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

        // 좌우 방향으로만 제한
        direction = new Vector3(direction.x, 0, 0).normalized;

        targetPosition = transform.position + direction * rollingDistance;

        // 롤링 효과음 재생
        PlayRollingSound();

        // Rigidbody2D의 중력을 활성화
        rb2d.gravityScale = 1;

        // 트랜스폼을 이용하여 목표 위치로 부드럽게 이동
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time < startTime + rollingDuration)
        {
            // 목표 위치로 트랜스폼 이동
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, (Time.time - startTime) / rollingDuration);
            rb2d.MovePosition(newPosition); // MovePosition을 사용하여 부드럽게 이동

            yield return null;
        }

        // 롤링 종료 시 위치를 최종 목표 위치로 설정
        rb2d.MovePosition(targetPosition);
        rb2d.gravityScale = 1; // 중력 유지

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
