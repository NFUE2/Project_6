using UnityEngine;
using UnityEngine.InputSystem;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;
    protected Camera mainCamera;

    // 이거 거너만 쓰는거 같은데 그쪽으로 옮겨주세요, 다른애는 안쓰기 때문에 여기서 필요없을것같아요
    protected bool isAttackCooldown = false;
    protected int attackCount = 0; 
    protected float cooldownDuration = 0.5f; // 장전 속도
    //=========================================


    private void Start()
    {
        mainCamera = Camera.main;
    }

    public override void Attack()
    {
        if (isAttackCooldown) return;
        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //이 부분 PlayerInput에서 받아오고 그 데이터를 기준으로 처리해주세요

        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;
        GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);

        attackInstance.GetComponent<Projectile>().SetDirection(attackDirection); //투사체를 회전시켜 해당방향으로 갈수있도록 해주세요
    }

    //거너용 함수는 거너에게 갑시다
    protected void UpdateCooldown()
    {
        if (isAttackCooldown && Time.time - lastAttackTime >= cooldownDuration)
        {
            isAttackCooldown = false;
        }
    }
    //=====================================
}
