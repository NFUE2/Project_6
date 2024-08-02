using UnityEngine;
using System.Collections;

public class RollingSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    [SerializeField] private float rollingForce = 10f; // ������ ��
    [SerializeField] private float rollingDuration = 0.5f; // ������ ���� �ð�

    private bool isRolling = false; // �Ѹ� ������ ����
    private bool isInvincible = false; // ���� �������� ����
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cooldownDuration = PlayerData.SkillECooldown;
    }

    public override void UseSkill()
    {
        if (isRolling || Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("RollingSkill: Cannot use skill, either on cooldown or already rolling.");
            return;
        }

        Debug.Log("RollingSkill: Skill used.");
        StartCoroutine(Rolling());
    }

    private IEnumerator Rolling()
    {
        isRolling = true;
        isInvincible = true;
        lastActionTime = Time.time;

        // �ִϸ��̼� ���� (�ִϸ��̼� �ڵ� �߰� ����)
        // GetComponent<Animator>().SetTrigger("Roll");

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z�� �� ����
        Vector3 direction = (mousePosition - transform.position).normalized;
        direction.y = 0; // ���� �������θ� �������� ����
        Debug.Log($"RollingSkill: Calculated direction: {direction}");

        rb.AddForce(direction * rollingForce, ForceMode2D.Impulse);
        Debug.Log($"RollingSkill: Applied force: {direction * rollingForce}");

        yield return new WaitForSeconds(rollingDuration);

        rb.velocity = Vector2.zero;
        Debug.Log("RollingSkill: Rolling ended.");

        isRolling = false;
        isInvincible = false;

        // �ִϸ��̼� ���� (�ִϸ��̼� �ڵ� �߰� ����)
        // GetComponent<Animator>().SetTrigger("StopRoll");
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
