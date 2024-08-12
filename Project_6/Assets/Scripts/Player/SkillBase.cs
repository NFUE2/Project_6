using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class SkillBase : MonoBehaviour
{
    protected float lastActionTime;
    public TextMeshProUGUI cooldownText;
    public Image cooldownImage;
    //public Image coolTime;
    protected float cooldownDuration; // ��ü ��Ÿ��

    public void SetCooldownText(TextMeshProUGUI text)
    {
        cooldownText = text;
    }

    public void SetCooldownImage(Image image)
    {
        cooldownImage = image;
    }

    protected void UpdateCooldownText()
    {
        if (cooldownText != null)
        {
            if (Time.time - lastActionTime >= cooldownDuration)
            {
                cooldownText.text = "�غ� �Ϸ�";
            }
            else
            {
                float remainingTime = cooldownDuration - (Time.time - lastActionTime);
                cooldownText.text = $"{remainingTime:F1}";
            }
        }
    }

    protected void UpdateCooldownImage()
    {
        if(cooldownImage != null)
        {
            if(Time.time - lastActionTime >= cooldownDuration)
            {
                cooldownImage.fillAmount = 0;
            }
            else
            {
                float remainingTime = cooldownDuration - (Time.time - lastActionTime);
                float coolFillAmount = remainingTime / cooldownDuration;
                cooldownImage.fillAmount = coolFillAmount;
            }
        }
    }


    private void Update()
    {
        UpdateCooldownImage();
    }

    public abstract void UseSkill();
}
