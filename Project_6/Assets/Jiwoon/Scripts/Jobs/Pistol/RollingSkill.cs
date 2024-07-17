using UnityEngine;
using TMPro;

public class RollingSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    public float rollingX;

    public bool IsRolling { get; private set; }
    public bool isInvincible { get; private set; }

    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        cooldownDuration = PlayerData.SkillECooldown;
    }

    public override void UseSkill()
    {
        if (IsRolling || Time.time - lastActionTime < cooldownDuration) return;

        IsRolling = true;
        isInvincible = true;

        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        int c = dir.x > 0 ? 1 : -1;

        transform.position += new Vector3(rollingX * c, 0, 0) * Time.deltaTime;

        Invoke("ExitRolling", 1.0f);
    }

    private void ExitRolling()
    {
        IsRolling = false;
        isInvincible = false;
        rigidbody.velocity = Vector2.zero;
        lastActionTime = Time.time;
    }
}
