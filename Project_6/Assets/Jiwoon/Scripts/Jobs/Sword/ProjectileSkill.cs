using UnityEngine;
using TMPro;
public class ProjectileSkill : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform attackPoint;
    public PlayerData PlayerData;
    private float lastActionTime;
    private TextMeshProUGUI cooldownText;

    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }

    public void UseSkill()
    {
        if (Time.time - lastActionTime < PlayerData.SkillECooldown) return; // E 스킬 쿨타임 체크
        Debug.Log("E 스킬 사용");

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GameObject projectileInstance = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().SetDirection(dir);

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