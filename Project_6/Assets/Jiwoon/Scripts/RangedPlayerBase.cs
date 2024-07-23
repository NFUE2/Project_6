using UnityEngine;
using UnityEngine.InputSystem;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInput playerInput; // �ν����Ϳ��� �Ҵ��� �� �ֵ��� ����

    private void Start()
    {
        mainCamera = Camera.main;

        // �ν����Ϳ��� playerInput�� �������� �ʾҴٸ� GetComponent�� �Ҵ� �õ�
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput ������Ʈ�� GameObject�� �����ϴ�.");
        }
        else
        {
            Debug.Log("PlayerInput ������Ʈ�� ���������� �ʱ�ȭ�Ǿ����ϴ�.");
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera�� �Ҵ���� �ʾҰų� ã�� �� �����ϴ�.");
        }
        else
        {
            Debug.Log("Main Camera�� ���������� �ʱ�ȭ�Ǿ����ϴ�.");
        }
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < playerData.attackCooldown) return;
        lastAttackTime = Time.time;

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }
        Debug.Log(mainCamera);
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(playerInput.GetMousePosition());
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;
        GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);

        attackInstance.GetComponent<Projectile>().SetDirection(attackDirection);
    }
}
