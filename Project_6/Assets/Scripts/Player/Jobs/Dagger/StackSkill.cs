using UnityEngine;
using System.Collections;

public class StackSkill : SkillBase
{
    public int currentStack = 0;
    public int maxStack = 10;
    public int damagePerStack = 10;
    public LayerMask enemyLayer;
    public Vector2 attackSize; // 공격 박스 크기
    public Vector2 attackOffset; // 공격 박스 오프셋
    public Animator animator; // Animator 추가
    public AudioClip skillSound; // 스킬 효과음 추가

    private AudioSource audioSource;
    private Transform playerTransform;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        playerTransform = transform;
    }

    public override void UseSkill()
    {
        if (currentStack > 0 && Time.time - lastActionTime >= cooldownDuration)
        {
            PlaySkillSound();
            animator.SetTrigger("IsAttack");
            DealDamageWithStack();
            currentStack = 0;
            lastActionTime = Time.time;
        }
    }

    public IEnumerator StackIncreaseCheck(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);

        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        bool hasIncreasedStack = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<IDamagable>(out var damagable) && !hasIncreasedStack)
            {
                IncreaseStack();
                hasIncreasedStack = true;
            }
        }
    }

    private void PlaySkillSound()
    {
        if (skillSound != null)
        {
            audioSource.PlayOneShot(skillSound);
        }
    }

    private void DealDamageWithStack()
    {
        int totalDamage = currentStack * damagePerStack;
        DealDamageToEnemies(totalDamage);
    }

    private void DealDamageToEnemies(int damage)
    {
        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.transform.TryGetComponent(out IPunDamagable m))
                m.Damage(damage);
        }
    }

    private Vector2 CalculateAttackPosition()
    {
        Vector2 basePosition = (Vector2)playerTransform.position;

        return playerTransform.localScale.x < 0
            ? basePosition + new Vector2(-attackOffset.x, attackOffset.y) // 왼쪽을 바라볼 때
            : basePosition + attackOffset; // 오른쪽을 바라볼 때
    }

    public void IncreaseStack()
    {
        currentStack = Mathf.Min(currentStack + 1, maxStack);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }

    private void Update()
    {
        UpdateCooldownImage();
        UpdateCooldownText();
    }
}
