using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    private Vector2 direction;

    public void SetDirection(Vector2 newDirection) //해당 함수 삭제
    {
        direction = newDirection.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime; //direction이 아니라 고정적인 벡터값으로 변경해주세요
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌 처리 코드
        // 예: 적에게 피해를 입히거나 투사체를 파괴
    }
}
