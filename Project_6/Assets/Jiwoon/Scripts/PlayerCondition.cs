using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Image healthBarImage; // ü�¹� �̹���

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage: " + damage + ", current health: " + currentHealth);

        UpdateHealthBar(); // ü�¹� ������Ʈ

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;
        healthBarImage.fillAmount = healthRatio; // ü�¹��� fillAmount �����Ѵ�.
    }

    void Die()
    {
        Debug.Log("Player died!");
        Destroy(gameObject);
    }
}
