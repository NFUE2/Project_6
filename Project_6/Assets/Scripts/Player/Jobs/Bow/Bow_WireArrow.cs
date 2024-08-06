using UnityEngine;
using Photon.Pun;

public class Bow_WireArrow : MonoBehaviour
{
    public float arrowSpeed = 20f; // 화살 속도
    public float wireSpeed = 10f; // 와이어 속도
    private bool isCollision;

    public Transform player;
    public float wireEndDistance = 0.5f;
    public Collider2D ignoreObject;

    private Rigidbody2D rb;
    private Collider2D arrowCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        arrowCollider = GetComponent<Collider2D>();

        Invoke("DestroyObject", 5.0f);

        //if (player == null)
        //{
        //    Debug.LogError("Player Transform이 할당되지 않았습니다.");
        //}

        // 플레이어와의 충돌 무시
        //Collider2D playerCollider = player.GetComponent<Collider2D>();
        //if (playerCollider != null)
        //{
        //    Physics2D.IgnoreCollision(arrowCollider, playerCollider, true);
        //}

        //if (ignoreObject != null)
        //{
        //    Physics2D.IgnoreCollision(arrowCollider, ignoreObject, true);
        //}
    }

    private void Update()
    {
        if (!isCollision)
        {
            rb.velocity = transform.right * arrowSpeed;
        }
        else
        {
            if (player != null)
            {
                player.position = Vector2.Lerp(player.position, transform.position, Time.deltaTime * wireSpeed);
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                if (Vector3.Distance(transform.position, player.position) < wireEndDistance)
                {
                    DestroyObject();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌 발생: " + collision.gameObject.name);

        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player와 충돌 무시: " + collision.gameObject.name);
            return;
        }

        if (ignoreObject != null && collision.collider == ignoreObject)
        {
            Debug.Log("무시할 오브젝트와 충돌: " + collision.gameObject.name);
            return;
        }

        Debug.Log("충돌한 오브젝트: " + collision.gameObject.name);

        isCollision = true;
        rb.velocity = Vector2.zero;
    }

    private void DestroyObject()
    {
        //Destroy(gameObject);
        PhotonNetwork.Destroy(gameObject);
    }
}
