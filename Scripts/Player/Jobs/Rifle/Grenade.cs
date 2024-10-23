using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviourPun
{
    [Header("Grenade Settings")]
    public float damage = 10f; // ����ź�� ������
    public float radius = 5f; // ���� �ݰ�
    public float dotDuration = 3f; // ������ ���� Ÿ��(DoT) ���� �ð�
    public float speed = 15f; // ����ź�� �ӵ�

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

    private void Awake()
    {
        // ����� �ҽ� �ʱ�ȭ
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        // ����ü�� �̵� ������ �������� �ʾ��� ��� �ʱ� ���� ����
        if (direction == Vector2.zero)
        {
            direction = Vector2.right; // �⺻ ������ ���������� ����
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // ����ü�� �������� ȸ����Ŵ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Update()
    {
        // ����ź�� �̵� ó��
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // ����ź�� ȭ�� ������ ������ �ı�
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
        // �浹�� ������Ʈ�� ���̾ "Enemy"���� Ȯ��
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            direction = Vector2.zero;
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
            ParticleSystem explosionInstance = Instantiate(explosionEffect, transform.position, transform.rotation);
            var mainModule = explosionInstance.main;
            mainModule.stopAction = ParticleSystemStopAction.Destroy;
            explosionInstance.Play();
        }

        // ���� �ݰ� ���� ��� ���� ã��
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayerMask);

        foreach (var enemy in hitEnemies)
        {
            // ������ DoT �������� ����
            if (enemy.TryGetComponent(out MonsterCondition monsterCondition))
            {
                StartCoroutine(ApplyDotDamage(monsterCondition));
            }
        }

        // ��Ʈ �� ���� �� ����ź ������Ʈ �ı�
        StartCoroutine(DestroyAfterDotDuration());
    }

    private IEnumerator ApplyDotDamage(IPunDamagable enemy)
    {
        float elapsed = 0f;
        float interval = 1f;
        float dotDamage = damage / dotDuration;

        // Ÿ�� ���� ȿ���� ��� ����
        audioSource.clip = burningSound;
        audioSource.loop = true;
        audioSource.Play();

        // Ÿ�� ���� ��ƼŬ ȿ�� ����
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

        // Ÿ�� ���� ȿ���� ����
        audioSource.Stop();

        // Ÿ�� ���� ��ƼŬ ȿ�� ���� �� �ı�
        if (burningInstance != null)
        {
            burningInstance.Stop();
            Destroy(burningInstance.gameObject, burningInstance.main.duration);
        }
    }

    private IEnumerator DestroyAfterDotDuration()
    {
        // ��Ʈ �� ���� �ð��� ���� �� ����ź ������Ʈ �ı�
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
