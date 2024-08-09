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
            Debug.Log("플레이어가 죽어서 힐이 안됩니다.");
            return; // 플레이어가 죽었으면 힐하지 않음
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력 범위 제한
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
        MakePlayerTransparent(); // 플레이어를 반투명하게 만들기
    }

    private void MakePlayerTransparent()
    {
        // 플레이어와 모든 자식 오브젝트의 SpriteRenderer를 가져옴
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // 각 SpriteRenderer의 색상을 변경하여 반투명하게 설정
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = 0.5f; // 알파 값을 낮춰 반투명 상태로 만듦
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
