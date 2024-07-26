using UnityEngine;

public class Bow_WireArrow : MonoBehaviour
{
    public float arrowSpeed = 20f; // ȭ�� �ӵ�
    public float wireSpeed = 10f; // ���̾� �ӵ�
    private bool isCollision;

    public Transform player;
    public float wireEndDistance = 0.5f;
    public Collider2D ignoreObject;

    // PhotonView pv;

    private void Start()
    {
        // pv = GetComponent<PhotonView>();
        Invoke("DestroyObject", 5.0f);

        if (player == null)
        {
            Debug.LogError("Player Transform�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // �浹 ���� ó�� (������)
        // if (ignoreObject != null)
        // {
        //     Collider2D[] ignoreColliders = ignoreObject.GetComponents<Collider2D>();
        //     foreach (Collider2D ignoreCollider in ignoreColliders)
        //     {
        //         Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignoreCollider, true);
        //     }
        // }
    }

    private void Update()
    {
        if (!isCollision)
        {
            // �浹�� �߻����� ���� ��� ȭ�� �̵�
            transform.position += transform.right * arrowSpeed * Time.deltaTime;
        }
        else
        {
            // �浹�� �߻��� ��� �÷��̾ ȭ�� ��ġ�� �̵�
            if (player != null)
            {
                player.position = Vector2.Lerp(player.position, transform.position, Time.deltaTime * wireSpeed);
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                // �÷��̾ ȭ�� ��ġ�� �����ϸ� ������Ʈ �ı�
                if (Vector3.Distance(transform.position, player.position) < wireEndDistance)
                {
                    // pv.RPC("DestroyObject", RpcTarget.All);
                    DestroyObject(); // Photon RPC ��� ���÷� ������Ʈ �ı�
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("�浹 �߻�: " + collision.gameObject.name); // �浹 ����� �α�

        // Player �±׸� ���� ������Ʈ���� �浹�� ����
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player�� �浹 ����: " + collision.gameObject.name);
            return;
        }

        // ������ ignoreObject�� �浹�� ��� ����
        if (ignoreObject != null && collision == ignoreObject)
        {
            Debug.Log("������ ������Ʈ�� �浹: " + collision.gameObject.name);
            return;
        }

        // �ٸ� ������Ʈ�� �浹�� ���
        Debug.Log("�浹�� ������Ʈ: " + collision.gameObject.name); // �浹 ����� �α�

        isCollision = true; // �浹 �߻� �� isCollision�� true�� ����
    }

    // [PunRPC]
    private void DestroyObject()
    {
        Destroy(gameObject); // ������Ʈ �ı�
    }
}


