using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    public float speed = 15f; // ����ü�� �ӵ�
    private Vector2 direction; // ����ü�� ����
    public int damage = 10; // ����ü�� ������ ������
    public GameObject explosionEffect; // ���� ȿ�� ������
    public float explosionLifetime = 2.0f; // ���� ȿ���� ���� �ð�
    public AudioClip explosionSound; // ���� ȿ����

    public float lifetime = 5.0f; // ����ü�� ���� �ð�

    private void OnEnable()
    {
        // ������ ���� �ð��� ������ ����ü�� �ı�
        Invoke("DestroyProjectile", lifetime);
    }

    private void OnDisable()
    {
        // ������Ʈ�� ��Ȱ��ȭ�� �� Invoke ���
        CancelInvoke("DestroyProjectile");
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
        // �̸� ���� �������� �̵�
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
        // ���� ȿ�� ���
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(effect, explosionLifetime); // ���� �ð��� ������ ���� ȿ�� �ı�
        }

        // ���� ȿ���� ���
        if (explosionSound != null)
        {
            SoundManager.Instance.Shot(explosionSound); // SoundManager�� ���� ȿ���� ���
        }

        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        // Ǯ�� �ý����� ����� ��쿡�� ��Ȱ��ȭ, �׷��� �ʴٸ� �ı�
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
