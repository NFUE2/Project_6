using UnityEngine;
using TMPro;

public abstract class SkillBase : MonoBehaviour
{
    protected float lastActionTime;
    protected TextMeshProUGUI cooldownText;
    protected float cooldownDuration;

    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }

    protected void UpdateCooldownText()
    {
        if (cooldownText != null)
        {
            if (Time.time - lastActionTime >= cooldownDuration)
            {
                cooldownText.text = "쿨타임 완료";
            }
            else
            {
                float remainingTime = cooldownDuration - (Time.time - lastActionTime);
                cooldownText.text = $"{remainingTime:F1}";
            }
        }
    }

    private void Update()
    {
        UpdateCooldownText();
    }

    public abstract void UseSkill();
}
