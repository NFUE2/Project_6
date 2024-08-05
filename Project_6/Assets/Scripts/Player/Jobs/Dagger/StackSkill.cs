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
    public Vector2 attackSize; // ���� �ڽ� ũ��
    public Vector2 attackOffset; // ���� �ڽ� ������
    public Animator animator; // Animator �߰�
    public AudioClip skillSound; // ��ų ȿ���� �߰�
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
            PlaySkillSound(); // ��ų ���� ���
            animator.SetTrigger("IsAttack"); // �Ϲ� ���� �ִϸ��̼� Ʈ����
            DealDamageWithStack();
            currentStack = 0;
            lastActionTime = Time.time;
        }
        else
        {
            Debug.Log($"������ �����մϴ�. ���� ���� : {currentStack}");
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
        Debug.Log($"���� {currentStack}���� ����Ͽ� {totalDamage}�� �������� �������ϴ�.");

        DealDamageToEnemies(totalDamage);
    }

    private void DealDamageToEnemies(int damage)
    {
        Vector2 attackPosition = CalculateAttackPosition();
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.transform.TryGetComponent(out MonsterCondition m))
                m.Damage(damage);
            //enemy.GetComponent<IDamagable>()?.TakeDamage(damage);
            //Debug.Log($"�� {enemy.name}���� {damage}�� �������� �������ϴ�.");
        }
    }

    private Vector2 CalculateAttackPosition()
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

    public void IncreaseStack()
    {
        currentStack++;
        if (currentStack > maxStack)
        {
            currentStack = maxStack;
        }
        Debug.Log($"���� ����! ���� ���� : {currentStack}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 attackPosition = CalculateAttackPosition();
        Gizmos.DrawWireCube(attackPosition, attackSize);
    }
}
