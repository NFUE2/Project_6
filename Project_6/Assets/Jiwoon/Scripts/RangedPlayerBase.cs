using UnityEngine;
using UnityEngine.InputSystem;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;
    protected Camera mainCamera;

    protected bool isAttackCooldown = false;
    protected int attackCount = 0;
    protected float cooldownDuration = 0.5f; // 장전 속도

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public override void Attack()
    {
        if (isAttackCooldown) return;
        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;
        GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);
        attackInstance.GetComponent<Projectile>().SetDirection(attackDirection);
    }

    protected void UpdateCooldown()
    {
        if (isAttackCooldown && Time.time - lastAttackTime >= cooldownDuration)
        {
            isAttackCooldown = false;
        }
    }
}
