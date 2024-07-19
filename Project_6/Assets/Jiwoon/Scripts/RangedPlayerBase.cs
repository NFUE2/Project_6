using UnityEngine;
using UnityEngine.InputSystem;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInput playerInput; // 인스펙터에서 할당할 수 있도록 설정

    private void Start()
    {
        mainCamera = Camera.main;

        // 인스펙터에서 playerInput이 설정되지 않았다면 GetComponent로 할당 시도
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput 컴포넌트가 GameObject에 없습니다.");
        }
        else
        {
            Debug.Log("PlayerInput 컴포넌트가 성공적으로 초기화되었습니다.");
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera가 할당되지 않았거나 찾을 수 없습니다.");
        }
        else
        {
            Debug.Log("Main Camera가 성공적으로 초기화되었습니다.");
        }
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < playerData.attackCooldown) return;
        lastAttackTime = Time.time;

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput이 초기화되지 않았습니다.");
            return;
        }
        Debug.Log(mainCamera);
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(playerInput.GetMousePosition());
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;
        GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);

        attackInstance.GetComponent<Projectile>().SetDirection(attackDirection);
    }
}
