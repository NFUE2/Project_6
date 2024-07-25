using UnityEngine;

public class Bow : MonoBehaviour
{
    private Camera mainCamera;
    public Transform playerTransform; // 플레이어의 Transform

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f; // 각도 조정

        // 플레이어가 좌측을 보고 있는지 우측을 보고 있는지 확인
        bool isPlayerFacingRight = playerTransform.localScale.x > 0;

        // 활의 회전을 플레이어의 방향에 따라 조정
        if (isPlayerFacingRight)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180f));
        }
    }
}
