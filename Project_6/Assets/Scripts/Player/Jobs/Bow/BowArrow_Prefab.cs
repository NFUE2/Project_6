using UnityEngine;

public class BombArrow : MonoBehaviour
{
    public float speed = 15f; // ȭ�� �ӵ�
    private Vector2 direction; // ȭ�� ����
    public int damage = 10; // �⺻ ������
    public float explosionRadius = 2.0f; // ���� �ݰ�
    public GameObject explosionEffect; // ���� ȿ�� ������
    public AudioClip explosionSound; // ���� ȿ����
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Collider2D arrowCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true; // ���� ������ ������ ���� �ʵ��� ����
        }

        arrowCollider = GetComponent<Collider2D>();
        if (arrowCollider == null)
        {
            arrowCollider = gameObject.AddComponent<CircleCollider2D>();
            arrowCollider.isTrigger = true; // Ʈ���ŷ� ����
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {

    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // ȭ���� �������� ȸ����Ŵ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ������ �����Ͽ� ȭ���� �ùٸ� �������� ���ϵ��� ��
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        //transform.position += (Vector3)direction * speed * Time.deltaTime;
        transform.position += transform.right * speed * Time.deltaTime;


        // ȭ���� ȭ�� ������ ������ �ı�
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        // ���� ȿ�� ���
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(effect, 2.0f); // ���� �ð��� ������ ���� ȿ�� �ı�
        }

        // ���� ȿ���� ���
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }
        else
        {
            Debug.LogError("explosionSound �Ǵ� audioSource�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // ���� �ݰ� ���� ������ ������ ����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in colliders)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                IDamagable damagable = hit.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                    Debug.Log($"�� {hit.gameObject.name}���� {damage}�� �������� �������ϴ�.");
                }
            }
        }

        // ȭ�� �ı�
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        // ���� �ݰ��� �ð������� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
