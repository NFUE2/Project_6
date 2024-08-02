using UnityEngine;
using System.Collections;

public class RollingSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    [SerializeField] private float rollingForce = 10f; // 구르는 힘
    [SerializeField] private float rollingDuration = 0.5f; // 구르는 지속 시간

    private bool isRolling = false; // 롤링 중인지 여부
    private bool isInvincible = false; // 무적 상태인지 여부
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

        // 애니메이션 시작 (애니메이션 코드 추가 예정)
        // GetComponent<Animator>().SetTrigger("Roll");

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z축 값 무시
        Vector3 direction = (mousePosition - transform.position).normalized;
        direction.y = 0; // 수평 방향으로만 구르도록 설정
        Debug.Log($"RollingSkill: Calculated direction: {direction}");

        rb.AddForce(direction * rollingForce, ForceMode2D.Impulse);
        Debug.Log($"RollingSkill: Applied force: {direction * rollingForce}");

        yield return new WaitForSeconds(rollingDuration);

        rb.velocity = Vector2.zero;
        Debug.Log("RollingSkill: Rolling ended.");

        isRolling = false;
        isInvincible = false;

        // 애니메이션 종료 (애니메이션 코드 추가 예정)
        // GetComponent<Animator>().SetTrigger("StopRoll");
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
