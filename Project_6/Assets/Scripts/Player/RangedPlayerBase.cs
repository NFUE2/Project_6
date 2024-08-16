using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;

    public override void Attack()
    {
        if (currentAttackTime < playerData.attackTime) return;
        currentAttackTime = 0f; // 공격 시 쿨타임 초기화

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        GameObject projectile = PhotonNetwork.Instantiate(attackPrefab.name, attackPoint.position, Quaternion.Euler(0, 0, angle));

        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDirection(attackDirection); // 투사체의 방향 설정
        }

        PlaySound(attackSound); // 공격 효과음 재생
    }
}
