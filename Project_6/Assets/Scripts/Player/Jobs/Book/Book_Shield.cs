using UnityEngine;

public class Book_Shield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            Destroy(collision.gameObject);
            Debug.Log("��ȣ���� ���� ���ݰ� �浹�Ͽ� ���� ������ �ı��߽��ϴ�.");
        }
    }
}
