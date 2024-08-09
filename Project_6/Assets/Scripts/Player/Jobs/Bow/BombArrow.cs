using Photon.Pun;
using UnityEngine;

public class BombArrow : MonoBehaviourPun
{
    public float speed = 15f; // 화살 속도
    private Vector2 direction; // 화살 방향
    public int damage = 10; // 기본 데미지
    public float explosionRadius = 2.0f; // 폭발 반경
    public GameObject explosionEffect; // 폭발 효과 프리팹
    public AudioClip explosionSound; // 폭발 효과음
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Collider2D arrowCollider;

    void Awake()
    {
        // Rigidbody2D 캐싱 및 초기화
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true; // 물리 엔진의 영향을 받지 않도록 설정
        }

        // Collider2D 캐싱 및 초기화
        arrowCollider = GetComponent<Collider2D>();
        if (arrowCollider == null)
        {
            arrowCollider = gameObject.AddComponent<CircleCollider2D>();
            arrowCollider.isTrigger = true; // 트리거로 설정
        }

        // AudioSource 캐싱 및 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // 화살을 방향으로 회전시킴
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        // 화살의 이동 처리
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // 화살이 화면 밖으로 나가면 파괴
        if (IsOffScreen())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOffScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < -0.5f || screenPoint.x > 1.5f || screenPoint.y < -0.5f || screenPoint.y > 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        // 폭발 효과 재생
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(effect, 2.0f); // 일정 시간이 지나면 폭발 효과 파괴
        }

        // 폭발 효과음 재생
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // 폭발 반경 내의 적에게 데미지 입힘
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in colliders)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //IDamagable damagable = hit.GetComponent<IDamagable>();
                IPunDamagable damagable = hit.GetComponent<IPunDamagable>();

                if (damagable != null)
                {
                    damagable.Damage(damage);
                }
            }
        }

        // 화살 파괴
        //Destroy(gameObject);
        if(photonView.IsMine)PhotonNetwork.Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        // 폭발 반경을 시각적으로 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
