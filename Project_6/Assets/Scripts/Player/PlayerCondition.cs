using Photon.Pun;
using Unity.VisualScripting;
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
    }

    void Update()
    {
        // �ʿ� �� �߰� ������Ʈ ����
    }

    public void TakeDamage(float damage)
    {
        if (input.isDead)
        {
            return;
        }

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
        if (photonView.IsMine) photonView.RPC(nameof(DieRpc), RpcTarget.All);
    }

    [PunRPC]
    void DieRpc()
    {
        input.isDead = true;
        GameManager.instance.PlayerDie();
        MakePlayerTransparent(); // �÷��̾ �������ϰ� �����
    }

    private void MakePlayerTransparent()
    {
        // ���� ������Ʈ�� ��� �ڽ� ������Ʈ�� SpriteRenderer�� ������
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        // �� SpriteRenderer�� ������ �����Ͽ� �������ϰ� ����
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = input.isDead ? 0.5f : 1.0f; // ���� ���� ���� ������ ���·� ����
            spriteRenderer.color = color;
        }
    }

    public void ModifyDefense(float amount)
    {
        PlayerData.playerdefense += amount;
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

    public void Resurrection()
    {
        photonView.RPC(nameof(ResurrectionRPC),RpcTarget.All);
        UpdateHealthBar();
    }

    [PunRPC]
    private void ResurrectionRPC()
    {
        currentHealth = maxHealth;
        input.isDead = false;
        MakePlayerTransparent();
    }
}
