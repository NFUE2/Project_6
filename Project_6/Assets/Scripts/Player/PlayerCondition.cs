using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviourPun, IDamagable, IKnockBackable
{
    public float maxHealth = 100f;
    public float currentHealth;

    public PlayerDataSO PlayerData;
    public PlayerInput input;
    public Image healthBarImage;
    private SpriteRenderer spriteRenderer;
    public AudioClip hitSound;
    private AudioSource audioSource;
    public GameObject hitEffectPrefab;

    private Rigidbody2D rb;
    public float knockbackForce = 5f;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        //UpdateHealthBar();
    }

    void Update()
    {
        //UpdateHealthBar();
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

        PlaySound(hitSound);
        PlayHitEffect();
        if (healthBarImage != null)
        {
            UpdateHealthBar();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        photonView.RPC(nameof(HealRPC), RpcTarget.All, amount);
    }

    [PunRPC]
    public void HealRPC(float amount)
    {
        if (input.isDead)
        {
            Debug.Log("�÷��̾ �׾ ���� �ȵ˴ϴ�.");
            return; // �÷��̾ �׾����� ������ ����
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ü�� ���� ����
        UpdateHealthBar();
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
        if (!photonView.IsMine) return;

        float healthRatio = currentHealth / maxHealth;
        healthBarImage.fillAmount = healthRatio;
    }

    void Die()
    {
        Debug.Log("Player died!");
        input.isDead = true;
        MakePlayerTransparent(); // �÷��̾ �������ϰ� �����
    }

    private void MakePlayerTransparent()
    {
        // �÷��̾�� ��� �ڽ� ������Ʈ�� SpriteRenderer�� ������
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // �� SpriteRenderer�� ������ �����Ͽ� �������ϰ� ����
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = 0.5f; // ���� ���� ���� ������ ���·� ����
            spriteRenderer.color = color;
        }
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
