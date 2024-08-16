using UnityEngine;
using Photon.Pun;

public class Bow_WireArrow : MonoBehaviourPun
{
    public float arrowSpeed = 20f; // ȭ�� �ӵ�
    public float wireSpeed = 10f; // ���̾� �ӵ�
    private bool isCollision;

    public Transform player;
    public float wireEndDistance = 0.5f;
    public LayerMask ignoreLayers; // �浹�� ������ ���̾��

    private Rigidbody2D rb;
    private Collider2D arrowCollider;
    private Vector2 direction; // ȭ���� �̵� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        arrowCollider = GetComponent<Collider2D>();

        // �÷��̾���� �浹 ����
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
        // ȭ���� �������� ȸ����Ŵ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Update()
    {
        if (!isCollision)
        {
            // ȭ�� �̵�
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
