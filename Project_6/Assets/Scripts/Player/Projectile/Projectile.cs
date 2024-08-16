using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    public float speed = 15f; // 투사체의 속도
    private Vector2 direction; // 투사체의 방향
    public int damage = 10; // 투사체가 입히는 데미지
    public GameObject explosionEffect; // 폭발 효과 프리팹
    public float explosionLifetime = 2.0f; // 폭발 효과의 생존 시간
    public AudioClip explosionSound; // 폭발 효과음

    public float lifetime = 5.0f; // 투사체의 생존 시간

    private void OnEnable()
    {
        // 지정된 생존 시간이 지나면 투사체를 파괴
        Invoke("DestroyProjectile", lifetime);
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 Invoke 취소
        CancelInvoke("DestroyProjectile");
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;

        // 투사체를 방향으로 회전시킵니다.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        // 미리 계산된 방향으로 이동
        Vector3 moveDirection = transform.right * speed * Time.deltaTime;
        transform.position += moveDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IPunDamagable damagable = collision.GetComponent<IPunDamagable>();

            if (damagable != null)
            {
                damagable.Damage(damage);
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
            SoundManager.Instance.Shot(explosionSound); // SoundManager를 통해 효과음 재생
        }

        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        // 풀링 시스템을 사용할 경우에는 비활성화, 그렇지 않다면 파괴
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
