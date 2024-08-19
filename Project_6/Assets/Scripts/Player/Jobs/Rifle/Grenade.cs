using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviourPun
{
    [Header("Grenade Settings")]
    public float damage = 10f; // 수류탄의 데미지
    public float radius = 5f; // 폭발 반경
    public float dotDuration = 3f; // 데미지 오버 타임(DoT) 지속 시간
    public float speed = 15f; // 수류탄의 속도

    [Header("Layer Masks")]
    public LayerMask enemyLayerMask; // 적의 레이어 마스크

    [Header("Audio Clips")]
    public AudioClip explosionSound; // 폭발 효과음
    public AudioClip burningSound; // 타는 듯한 효과음

    [Header("Particle Effects")]
    public ParticleSystem explosionEffect; // 폭발 파티클 효과
    public ParticleSystem burningEffect; // 타는 듯한 파티클 효과

    private AudioSource audioSource; // 오디오 소스
    private Vector2 direction; // 투사체의 방향

    private void Awake()
    {
        // 오디오 소스 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        // 투사체가 이동 방향을 설정하지 않았을 경우 초기 방향 설정
        if (direction == Vector2.zero)
        {
            direction = Vector2.right; // 기본 방향을 오른쪽으로 설정
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // 투사체를 방향으로 회전시킴
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Update()
    {
        // 수류탄의 이동 처리
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // 수류탄이 화면 밖으로 나가면 파괴
        if (IsOffScreen())
        {
            if(photonView.IsMine) Destroy(gameObject);
        }
    }

    private bool IsOffScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < -0.5f || screenPoint.x > 1.5f || screenPoint.y < -0.5f || screenPoint.y > 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트의 레이어가 "Enemy"인지 확인
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            direction = Vector2.zero;
            Explode();
        }
    }

    private void Explode()
    {
        // 폭발 효과음 재생
        audioSource.PlayOneShot(explosionSound);

        // 폭발 파티클 효과 시작
        if (explosionEffect != null)
        {
            ParticleSystem explosionInstance = Instantiate(explosionEffect, transform.position, transform.rotation);
            var mainModule = explosionInstance.main;
            mainModule.stopAction = ParticleSystemStopAction.Destroy;
            explosionInstance.Play();
        }

        // 폭발 반경 내의 모든 적을 찾음
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayerMask);

        foreach (var enemy in hitEnemies)
        {
            // 적에게 DoT 데미지를 적용
            if (enemy.TryGetComponent(out MonsterCondition monsterCondition))
            {
                StartCoroutine(ApplyDotDamage(monsterCondition));
            }
        }

        // 도트 딜 적용 후 수류탄 오브젝트 파괴
        StartCoroutine(DestroyAfterDotDuration());
    }

    private IEnumerator ApplyDotDamage(IPunDamagable enemy)
    {
        float elapsed = 0f;
        float interval = 1f;
        float dotDamage = damage / dotDuration;

        // 타는 듯한 효과음 재생 시작
        audioSource.clip = burningSound;
        audioSource.loop = true;
        audioSource.Play();

        // 타는 듯한 파티클 효과 시작
        ParticleSystem burningInstance = null;
        if (burningEffect != null)
        {
            burningInstance = Instantiate(burningEffect, transform.position, transform.rotation);
            var mainModule = burningInstance.main;
            mainModule.stopAction = ParticleSystemStopAction.Destroy;
            burningInstance.Play();
        }

        while (elapsed < dotDuration)
        {
            if (enemy != null)
            {
                //enemy.TakeDamage(dotDamage);
                enemy.Damage(dotDamage);
            }

            elapsed += interval;
            yield return new WaitForSeconds(interval);
        }

        // 타는 듯한 효과음 중지
        audioSource.Stop();

        // 타는 듯한 파티클 효과 중지 및 파괴
        if (burningInstance != null)
        {
            burningInstance.Stop();
            Destroy(burningInstance.gameObject, burningInstance.main.duration);
        }
    }

    private IEnumerator DestroyAfterDotDuration()
    {
        // 도트 딜 지속 시간이 지난 후 수류탄 오브젝트 파괴
        yield return new WaitForSeconds(dotDuration);
        if (photonView.IsMine) PhotonNetwork.Destroy(gameObject);
            // Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
