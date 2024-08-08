using System.Collections;
using UnityEngine;

public abstract class MeleePlayerBase : PlayerBase
{
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    public float attackCooldown = 1f;  // 공격 쿨타임 설정
    protected bool isAttacking = false;
    public Vector2 attackSize; // 공격 박스의 크기
    public Vector2 attackOffset; // 공격 박스의 오프셋
    public float knockbackForce = 5f; // 넉백 힘
    public AudioClip attackSound; // 공격 시 효과음
    public AudioClip hitSound; // 피격 시 효과음
    private AudioSource audioSource; // 오디오 소스

    protected void Awake() // 최상위 클래스에서 호출되도록 설정
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
        animator = GetComponentInChildren<Animator>();
    }

    public override void Attack()
    {
        if (isAttacking) return;  // 공격 중이 아닌 경우에만 공격
        isAttacking = true;
        animator.SetTrigger("IsAttack");
        StartCoroutine(AttackCooldown());
    }
    protected IEnumerator AttackCooldown() // 공격 쿨타임을 위한 코루틴
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false; // 쿨다운이 끝나면 isAttacking을 false로 설정
    }



    // 애니메이션 이벤트에서 호출될 메서드
    public void PerformAttack()
    {
        if (!isAttacking) return; // 공격 중이 아닐 때만 공격 수행

        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            //IDamagable damagable = enemy.GetComponent<IDamagable>();
            MonsterCondition damagable = enemy.GetComponent<MonsterCondition>();

            if (damagable != null)
            {
                //damagable.TakeDamage(attackDamage);
                damagable.Damage(attackDamage);
                ApplyKnockback(enemy);
                PlaySound(hitSound); // 피격 시 효과음 재생
            }
        }

        PlaySound(attackSound); // 실제 공격 시 효과음 재생
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

        if (transform.localScale.x < 0)
        {
            return basePosition + new Vector2(-attackOffset.x, attackOffset.y); // 왼쪽을 바라볼 때
        }
        else
        {
            return basePosition + attackOffset; // 오른쪽을 바라볼 때
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
