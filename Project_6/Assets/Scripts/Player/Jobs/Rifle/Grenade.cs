using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Grenade Settings")]
    public float damage = 10f; // ����ź�� ������
    public float radius = 5f; // ���� �ݰ�
    public float dotDuration = 3f; // ������ ���� Ÿ��(DoT) ���� �ð�

    [Header("Layer Masks")]
    public LayerMask enemyLayerMask; // ���� ���̾� ����ũ

    [Header("Audio Clips")]
    public AudioClip explosionSound; // ���� ȿ����
    public AudioClip burningSound; // Ÿ�� ���� ȿ����

    [Header("Particle Effects")]
    public ParticleSystem explosionEffect; // ���� ��ƼŬ ȿ��
    public ParticleSystem burningEffect; // Ÿ�� ���� ��ƼŬ ȿ��

    private AudioSource audioSource; // ����� �ҽ�
    private Vector2 direction; // ����ü�� ����
    private Rigidbody2D rb; // ������ٵ� ������Ʈ

    private void Start()
    {
        // ����� �ҽ� �ʱ�ȭ
        audioSource = gameObject.AddComponent<AudioSource>();

        // ������ٵ� ������Ʈ �ʱ�ȭ
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false; // ������ٵ� ���������� �����ϰ� ����
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // ����ü�� �������� ȸ����Ŵ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayerMask) != 0)
        {
            // ���� �浹 �� �ӵ� 0���� �����Ͽ� ����
            rb.velocity = Vector2.zero;

            // ���� �浹 �� ����
            Explode();
        }
    }

    private void Explode()
    {
        // ���� ȿ���� ���
        audioSource.PlayOneShot(explosionSound);

        // ���� ��ƼŬ ȿ�� ����
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // ���� �ݰ� ���� ��� ���� ã��
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayerMask);

        foreach (var enemy in hitEnemies)
        {
            // ������ DoT �������� ����
            IDamagable damagable = enemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                StartCoroutine(ApplyDotDamage(damagable));
            }
        }

        // ��Ʈ �� ���� �� ����ź ������Ʈ �ı�
        StartCoroutine(DestroyAfterDotDuration());
    }

    private IEnumerator ApplyDotDamage(IDamagable enemy)
    {
        float elapsed = 0f;
        float interval = 1f;
        float dotDamage = damage / dotDuration;

        // Ÿ�� ���� ȿ���� ��� ����
        audioSource.clip = burningSound;
        audioSource.loop = true;
        audioSource.Play();

        // Ÿ�� ���� ��ƼŬ ȿ�� ����
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

        // Ÿ�� ���� ȿ���� ����
        audioSource.Stop();

        // Ÿ�� ���� ��ƼŬ ȿ�� ���� �� �ı�
        if (effect != null)
        {
            effect.Stop();
            Destroy(effect.gameObject, effect.main.duration);
        }
    }

    private IEnumerator DestroyAfterDotDuration()
    {
        // ��Ʈ �� ���� �ð��� ���� �� ����ź ������Ʈ �ı�
        yield return new WaitForSeconds(dotDuration);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
