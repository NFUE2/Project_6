using UnityEngine;

public class Bow : MonoBehaviour
{
    private Camera mainCamera;
    public Transform playerTransform; // �÷��̾��� Transform

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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f; // ���� ����

        // �÷��̾ ������ ���� �ִ��� ������ ���� �ִ��� Ȯ��
        bool isPlayerFacingRight = playerTransform.localScale.x > 0;

        // Ȱ�� ȸ���� �÷��̾��� ���⿡ ���� ����
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
