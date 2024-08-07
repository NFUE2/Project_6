using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviourPun, IDamagable, IKnockBackable
{
    public float maxHealth = 100f;
    public float currentHealth;

    public PlayerDataSO PlayerData;
    public PlayerInput input;
    public Image healthBarImage; // Optional
    private Animator animator;
    public AudioClip hitSound;
    private AudioSource audioSource;
    public GameObject hitEffectPrefab;

    private Rigidbody2D rb;
    public float knockbackForce = 5f;

    void Start()
    {
        currentHealth = maxHealth; // 초기화
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        float damageAfterDefense;
        if (PlayerData != null)
        {
            damageAfterDefense = Mathf.Max(damage - PlayerData.playerdefense, 0);
        }
        else
        {
            damageAfterDefense = Mathf.Max(damage, 0);
        }
        currentHealth -= damageAfterDefense;
        Debug.Log("Player took damage: " + damageAfterDefense + ", current health: " + currentHealth);

        PlaySound(hitSound);
        PlayHitEffect();
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [PunRPC]
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력 범위 제한
        UpdateHealthBar();
        Debug.Log($"Heal applied: {amount}, current health: {currentHealth}");
    }

    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            float healthRatio = currentHealth / maxHealth;
            healthBarImage.fillAmount = healthRatio;
            Debug.Log("Health updated: " + currentHealth + "/" + maxHealth + " (" + healthRatio + ")");
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        input.isDead = true;
        animator.SetTrigger("ISDie");
    }

    public void ModifyDefense(float amount)
    {
        PlayerData.playerdefense += amount;
        Debug.Log("Defense modified by: " + amount + ", current defense: " + PlayerData.playerdefense);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1.0f);
        }
    }
}
