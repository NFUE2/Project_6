using UnityEngine;

public class Book_Shield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            Destroy(collision.gameObject);
            Debug.Log("보호막이 적의 공격과 충돌하여 적의 공격을 파괴했습니다.");
        }
    }
}
