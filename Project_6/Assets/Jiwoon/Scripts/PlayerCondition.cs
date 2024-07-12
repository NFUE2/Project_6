using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Image healthBarImage; // 체력바 이미지

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage: " + damage + ", current health: " + currentHealth);

        UpdateHealthBar(); // 체력바 업데이트

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;
        healthBarImage.fillAmount = healthRatio; // 체력바의 fillAmount 조정한다.
    }

    void Die()
    {
        Debug.Log("Player died!");
        Destroy(gameObject);
    }
}
