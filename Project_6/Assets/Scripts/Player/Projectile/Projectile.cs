using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f; // 투사체의 속도
    private Vector2 direction; // 투사체의 방향
    public int damage = 10; // 투사체가 입히는 데미지
    public float offScreenMargin = 0.5f; // 화면 밖으로 간주할 여백
    public GameObject explosionEffect; // 폭발 효과 프리팹
    public float explosionLifetime = 2.0f; // 폭발 효과의 생존 시간
    public AudioClip explosionSound; // 폭발 효과음
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // 투사체가 물리 엔진의 영향을 받지 않도록 설정

        // 오디오 소스 컴포넌트 가져오기 또는 추가하기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // SetDirection을 호출하여 초기 방향 설정
        // SetDirection 메서드는 외부에서 호출되어야 합니다.
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // 투사체를 방향으로 회전시킴
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
                Debug.Log($"적 {collision.gameObject.name}에게 {damage}의 데미지를 입혔습니다.");
            }
            Explode();
        }
    }

    private void Explode()
    {
        // 폭발 효과 재생
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(effect, explosionLifetime); // 일정 시간이 지나면 폭발 효과 파괴
        }

        // 폭발 효과음 재생
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        // 투사체 파괴
        Destroy(gameObject);
    }
}
