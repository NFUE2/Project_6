using UnityEngine;
using TMPro;
using System.Collections;

public class StackSkill : SkillBase
{
    public int currentStack = 0;
    public int maxStack = 10;
    public int damagePerStack = 10;
    public TextMeshPro stackText;
    public PlayerDataSO PlayerData;
    public LayerMask enemyLayer;
    public Vector2 attackSize; // 공격 박스 크기
    public Vector2 attackOffset; // 공격 박스 오프셋
    public Animator animator; // Animator 추가
    public AudioClip skillSound; // 스킬 효과음 추가
    private AudioSource audioSource;

    private void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        if (currentStack > 0)
        {
            PlaySkillSound(); // 스킬 사운드 재생
            animator.SetTrigger("IsAttack"); // 일반 공격 애니메이션 트리거
            DealDamageWithStack();
            currentStack = 0;
            lastActionTime = Time.time;
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
            if (enemy.transform.TryGetComponent(out IDamagable m))
                m.TakeDamage(damage);
            //enemy.GetComponent<IDamagable>()?.TakeDamage(damage);
        }
    }

    private Vector2 CalculateAttackPosition()
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

    public void IncreaseStack()
    {
        currentStack++;
        if (currentStack > maxStack)
        {
            currentStack = maxStack;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
