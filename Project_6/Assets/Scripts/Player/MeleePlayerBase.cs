using System.Collections;
using UnityEngine;

public abstract class MeleePlayerBase : PlayerBase
{
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    public float attackCooldown = 1f;  // ���� ��Ÿ�� ����
    protected bool isAttacking = false;
    public Vector2 attackSize; // ���� �ڽ��� ũ��
    public Vector2 attackOffset; // ���� �ڽ��� ������
    public float knockbackForce = 5f; // �˹� ��
    public AudioClip attackSound; // ���� �� ȿ����
    public AudioClip hitSound; // �ǰ� �� ȿ����
    private AudioSource audioSource; // ����� �ҽ�

    protected void Awake() // �ֻ��� Ŭ�������� ȣ��ǵ��� ����
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ��������
        animator = GetComponentInChildren<Animator>();
    }

    public override void Attack()
    {
        if (isAttacking) return;  // ���� ���� �ƴ� ��쿡�� ����
        isAttacking = true;
        animator.SetTrigger("IsAttack");
        StartCoroutine(AttackCooldown());
    }
    protected IEnumerator AttackCooldown() // ���� ��Ÿ���� ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false; // ��ٿ��� ������ isAttacking�� false�� ����
    }



    // �ִϸ��̼� �̺�Ʈ���� ȣ��� �޼���
    public void PerformAttack()
    {
        if (!isAttacking) return; // ���� ���� �ƴ� ���� ���� ����

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
                PlaySound(hitSound); // �ǰ� �� ȿ���� ���
            }
        }

        PlaySound(attackSound); // ���� ���� �� ȿ���� ���
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
            return basePosition + new Vector2(-attackOffset.x, attackOffset.y); // ������ �ٶ� ��
        }
        else
        {
            return basePosition + attackOffset; // �������� �ٶ� ��
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
