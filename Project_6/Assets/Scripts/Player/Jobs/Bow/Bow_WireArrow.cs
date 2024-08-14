using UnityEngine;
using Photon.Pun;

public class Bow_WireArrow : MonoBehaviourPun
{
    public float arrowSpeed = 20f; // 화살 속도
    public float wireSpeed = 10f; // 와이어 속도
    private bool isCollision;

    public Transform player;
    public float wireEndDistance = 0.5f;
    public LayerMask ignoreLayers; // 충돌을 무시할 레이어들

    private Rigidbody2D rb;
    private Collider2D arrowCollider;
    private Vector2 direction; // 화살의 이동 방향

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        arrowCollider = GetComponent<Collider2D>();

        // 플레이어와의 충돌 무시
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(arrowCollider, playerCollider, true);
            }
        }
    }

    private void Start()
    {
        Invoke("DestroyObject", 5.0f);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        // 화살을 방향으로 회전시킴
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Update()
    {
        if (!isCollision)
        {
            // 화살 이동
            transform.position += (Vector3)direction * arrowSpeed * Time.deltaTime;
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
        if (((1 << collision.collider.gameObject.layer) & ignoreLayers) != 0)
        {
            return;
        }
        isCollision = true;
        rb.velocity = Vector2.zero;
    }

    private void DestroyObject()
    {
        if(photonView.IsMine) PhotonNetwork.Destroy(gameObject);
    }
}
