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

    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // ����ü�� ���� ������ ������ ���� �ʵ��� ����

        // ����� �ҽ� ������Ʈ �������� �Ǵ� �߰��ϱ�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // SetDirection�� ȣ���Ͽ� �ʱ� ���� ����
        // SetDirection �޼���� �ܺο��� ȣ��Ǿ�� �մϴ�.
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // ����ü�� �������� ȸ����Ŵ
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
