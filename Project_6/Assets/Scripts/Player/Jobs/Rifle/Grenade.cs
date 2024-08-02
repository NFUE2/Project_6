using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Grenade Settings")]
    public float damage = 10f; // 수류탄의 데미지
    public float radius = 5f; // 폭발 반경
    public float dotDuration = 3f; // 데미지 오버 타임(DoT) 지속 시간

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
    private Rigidbody2D rb; // 리지드바디 컴포넌트

    private void Start()
    {
        // 오디오 소스 초기화
        audioSource = gameObject.AddComponent<AudioSource>();

        // 리지드바디 컴포넌트 초기화
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false; // 리지드바디를 물리적으로 반응하게 설정
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // 투사체를 방향으로 회전시킴
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayerMask) != 0)
        {
            // 적과 충돌 시 속도 0으로 설정하여 멈춤
            rb.velocity = Vector2.zero;

            // 적과 충돌 시 폭발
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
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // 폭발 반경 내의 모든 적을 찾음
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayerMask);

        foreach (var enemy in hitEnemies)
        {
            // 적에게 DoT 데미지를 적용
            IDamagable damagable = enemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                StartCoroutine(ApplyDotDamage(damagable));
            }
        }

        // 도트 딜 적용 후 수류탄 오브젝트 파괴
        StartCoroutine(DestroyAfterDotDuration());
    }

    private IEnumerator ApplyDotDamage(IDamagable enemy)
    {
        float elapsed = 0f;
        float interval = 1f;
        float dotDamage = damage / dotDuration;

        // 타는 듯한 효과음 재생 시작
        audioSource.clip = burningSound;
        audioSource.loop = true;
        audioSource.Play();

        // 타는 듯한 파티클 효과 시작
        ParticleSystem effect = null;
        if (burningEffect != null)
        {
            effect = Instantiate(burningEffect, transform.position, transform.rotation);
            effect.Play();
        }

        while (elapsed < dotDuration)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(dotDamage);
            }

            elapsed += interval;
            yield return new WaitForSeconds(interval);
        }

        // 타는 듯한 효과음 중지
        audioSource.Stop();

        // 타는 듯한 파티클 효과 중지 및 파괴
        if (effect != null)
        {
            effect.Stop();
            Destroy(effect.gameObject, effect.main.duration);
        }
    }

    private IEnumerator DestroyAfterDotDuration()
    {
        // 도트 딜 지속 시간이 지난 후 수류탄 오브젝트 파괴
        yield return new WaitForSeconds(dotDuration);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
