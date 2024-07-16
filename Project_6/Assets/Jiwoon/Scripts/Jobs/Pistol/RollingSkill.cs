using UnityEngine;
using TMPro;

public class RollingSkill : MonoBehaviour
{
    //상위에서처리
    public PlayerData PlayerData; //SkillDataSO 있습니다
    private float lastActionTime;
    private TextMeshProUGUI cooldownText;
    //======================================

    public float rollingX;

    //무적처리 필요, TakeDamage에서의 처리 필요
    public bool IsRolling { get; private set; }
    public bool isInvincible { get; private set; }
    //=================================================


    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    //쿨타임표기는 상위클래스에서 처리
    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }

    //오버라이드
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

    //상위클래스에서 처리
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
