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

        // 플레이어의 방향과 반대 방향으로 발사 방향 수정
        attackDirection = new Vector2(Mathf.Abs(attackDirection.x) * -Mathf.Sign(transform.localScale.x), attackDirection.y);

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        GameObject projectile = PhotonNetwork.Instantiate(attackPrefab.name, attackPoint.position, Quaternion.Euler(0, 0, angle));

        if (projectile.TryGetComponent<Projectile>(out var proj))
        {
            proj.SetDirection(attackDirection); // 투사체의 방향 설정
        }

        PlaySound(attackSound); // 공격 효과음 재생
    }



}
