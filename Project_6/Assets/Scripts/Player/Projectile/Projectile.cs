using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15; // 투사체의 속도
    private Vector2 direction; // 투사체의 방향
    public int damage = 10; // 투사체가 입히는 데미지
    public float offScreenMargin = 0.5f; // 화면 밖으로 간주할 여백

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // 투사체를 방향으로 회전시킴
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180; // 각도를 조정하여 투사체가 올바른 방향으로 향하도록 함
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // 투사체가 화면 밖으로 나가면 파괴
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
