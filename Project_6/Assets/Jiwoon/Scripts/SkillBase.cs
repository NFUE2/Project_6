using UnityEngine;
using TMPro;

public abstract class SkillBase : MonoBehaviour
{
    protected float lastActionTime;
    public TextMeshProUGUI cooldownText;
    protected float cooldownDuration;

    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }

    protected void UpdateCooldownText()
    {
        if (cooldownText != null)
        {
            Debug.Log(1);
            if (Time.time - lastActionTime >= cooldownDuration)
            {
                cooldownText.text = "준비 완료";
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
