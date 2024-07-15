using UnityEngine;
using TMPro;

public class RollingSkill : MonoBehaviour
{
    public float rollingX;
    public PlayerData PlayerData;
    private float lastActionTime;
    private TextMeshProUGUI cooldownText;
    public bool IsRolling { get; private set; }
    public bool isInvincible { get; private set; }
    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }

    public void UseSkill()
    {
        if (IsRolling) return;
        if (Time.time - lastActionTime < PlayerData.SkillECooldown) return;

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

    private void Update()
    {
        if (cooldownText != null)
        {
            if (Time.time - lastActionTime >= PlayerData.SkillECooldown)
            {
                cooldownText.text = "E스킬 쿨타임 완료"; // 쿨타임 완료 텍스트 갱신
            }
            else
            {
                float remainingTime = PlayerData.SkillECooldown - (Time.time - lastActionTime);
                cooldownText.text = $"{remainingTime:F1}"; // 쿨타임 텍스트 갱신
            }
        }
    }
}
