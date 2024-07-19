using UnityEngine;
using UnityEngine.InputSystem;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;
    protected Camera mainCamera;
    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < playerData.attackCooldown) return;
        lastAttackTime = Time.time;

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(playerInput.GetMousePosition());
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;
        GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);

        attackInstance.GetComponent<Projectile>().SetDirection(attackDirection);
    }
}
