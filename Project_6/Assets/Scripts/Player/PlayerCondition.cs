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
        // 필요 시 추가 업데이트 로직
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
        if (photonView.IsMine) photonView.RPC(nameof(DieRpc), RpcTarget.All);
    }

    [PunRPC]
    void DieRpc()
    {
        input.isDead = true;
        GameManager.instance.PlayerDie();
        MakePlayerTransparent(); // 플레이어를 반투명하게 만들기
    }

    private void MakePlayerTransparent()
    {
        // 현재 오브젝트와 모든 자식 오브젝트의 SpriteRenderer를 가져옴
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        // 각 SpriteRenderer의 색상을 변경하여 반투명하게 설정
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = input.isDead ? 0.5f : 1.0f; // 알파 값을 낮춰 반투명 상태로 만듦
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
