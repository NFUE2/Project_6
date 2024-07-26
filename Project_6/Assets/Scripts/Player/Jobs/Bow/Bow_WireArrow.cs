using UnityEngine;

public class Bow_WireArrow : MonoBehaviour
{
    public float arrowSpeed = 20f; // 화살 속도
    public float wireSpeed = 10f; // 와이어 속도
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
            Debug.LogError("Player Transform이 할당되지 않았습니다.");
        }

        // 충돌 무시 처리 (선택적)
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
            // 충돌이 발생하지 않은 경우 화살 이동
            transform.position += transform.right * arrowSpeed * Time.deltaTime;
        }
        else
        {
            // 충돌이 발생한 경우 플레이어를 화살 위치로 이동
            if (player != null)
            {
                player.position = Vector2.Lerp(player.position, transform.position, Time.deltaTime * wireSpeed);
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                // 플레이어가 화살 위치에 도달하면 오브젝트 파괴
                if (Vector3.Distance(transform.position, player.position) < wireEndDistance)
                {
                    // pv.RPC("DestroyObject", RpcTarget.All);
                    DestroyObject(); // Photon RPC 대신 로컬로 오브젝트 파괴
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌 발생: " + collision.gameObject.name); // 충돌 디버그 로그

        // Player 태그를 가진 오브젝트와의 충돌을 무시
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player와 충돌 무시: " + collision.gameObject.name);
            return;
        }

        // 지정된 ignoreObject와 충돌한 경우 무시
        if (ignoreObject != null && collision == ignoreObject)
        {
            Debug.Log("무시할 오브젝트와 충돌: " + collision.gameObject.name);
            return;
        }

        // 다른 오브젝트와 충돌한 경우
        Debug.Log("충돌한 오브젝트: " + collision.gameObject.name); // 충돌 디버그 로그

        isCollision = true; // 충돌 발생 시 isCollision을 true로 설정
    }

    // [PunRPC]
    private void DestroyObject()
    {
        Destroy(gameObject); // 오브젝트 파괴
    }
}


