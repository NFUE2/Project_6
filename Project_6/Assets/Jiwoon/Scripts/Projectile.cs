using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    private Vector2 direction;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹 ó�� �ڵ�
        // ��: ������ ���ظ� �����ų� ����ü�� �ı�
    }
}
