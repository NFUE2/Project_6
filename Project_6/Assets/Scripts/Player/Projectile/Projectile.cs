using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15; // ����ü�� �ӵ�
    private Vector2 direction; // ����ü�� ����
    public int damage = 10; // ����ü�� ������ ������
    public float offScreenMargin = 0.5f; // ȭ�� ������ ������ ����

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // ����ü�� �������� ȸ����Ŵ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180; // ������ �����Ͽ� ����ü�� �ùٸ� �������� ���ϵ��� ��
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // ����ü�� ȭ�� ������ ������ �ı�
        if (IsOffScreen())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOffScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < -offScreenMargin || screenPoint.x > 1 + offScreenMargin || screenPoint.y < -offScreenMargin || screenPoint.y > 1 + offScreenMargin;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.GetComponent<IDamagable>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
