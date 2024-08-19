using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class MeleePlayerBase : PlayerBase
{
    public LayerMask enemyLayer;
    public Vector2 attackSize;
    public Vector2 attackOffset;
    public float knockbackForce = 5f;
    public AudioClip hitSound;
    public GameObject hitEffectPrefab;
    public Animator animator;

    public override void Attack()
    {
        if (currentAttackTime < playerData.attackTime) return;
        currentAttackTime = 0f;
        animator.SetTrigger("IsAttack");
    }

    public void PerformAttack()
    {
        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            IPunDamagable damagable = enemy.GetComponent<IPunDamagable>();

            if (damagable != null)
            {
                damagable.Damage(playerData.attackDamage);
                ApplyKnockback(enemy);
                PlaySound(hitSound);

                if (hitEffectPrefab != null)
                {
                    Vector3 effectPosition = enemy.transform.position; // 적의 transform 위치 기준
                    effectPosition.y += 1f; // Y축으로 약간 위로 이동 (필요에 따라 조정)

                    Instantiate(hitEffectPrefab, effectPosition, Quaternion.identity);
                }
            }
        }

        PlaySound(attackSound);
    }



    protected void ApplyKnockback(Collider2D enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    protected Vector2 CalculateAttackPosition()
    {
        Vector2 basePosition = (Vector2)transform.position;

        // Transform의 localScale을 이용해 방향을 결정
        float direction = transform.localScale.x < 0 ? -1 : 1;

        return basePosition + new Vector2(attackOffset.x * direction, attackOffset.y);
    }






    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
