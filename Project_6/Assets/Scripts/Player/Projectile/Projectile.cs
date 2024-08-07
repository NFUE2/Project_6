using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f; // ����ü�� �ӵ�
    private Vector2 direction; // ����ü�� ����
    public int damage = 10; // ����ü�� ������ ������
    public float offScreenMargin = 0.5f; // ȭ�� ������ ������ ����
    public GameObject explosionEffect; // ���� ȿ�� ������
    public float explosionLifetime = 2.0f; // ���� ȿ���� ���� �ð�
    public AudioClip explosionSound; // ���� ȿ����
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    void Awake()
    {
        // ����� �ҽ� ������Ʈ�� ĳ���մϴ�.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;

        // ����ü�� �������� ȸ����ŵ�ϴ�.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        if (direction != Vector2.zero) // ������ �����Ǿ� ���� ���� ������Ʈ�մϴ�.
        {
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }

    private void OnBecameInvisible()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
                Debug.Log($"�� {collision.gameObject.name}���� {damage}�� �������� �������ϴ�.");
            }
            Explode();
        }
    }

    private void Explode()
    {
        // ���� ȿ�� ���
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(effect, explosionLifetime); // ���� �ð��� ������ ���� ȿ�� �ı�
        }

        // ���� ȿ���� ���
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        // ����ü �ı�
        Destroy(gameObject);
    }
}
