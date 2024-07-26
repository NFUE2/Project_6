using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public float maxHealth = 100f;
    private float currentHealth;

    public PlayerDataSO PlayerData; // PlayerDataSO 추가
    public PlayerInput input;
    public Image healthBarImage; // 체력바 이미지
    private Animator animator; // Animator 컴포넌트 추가
    public AudioClip hitSound; // 피격 시 효과음 추가
    private AudioSource audioSource; // AudioSource 컴포넌트 추가
    public GameObject hitEffect; // 피격 시 시각 효과 추가

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        float damageAfterDefense = Mathf.Max(damage - PlayerData.playerdefense, 0); // PlayerData의 방어력을 적용한 데미지 계산
        currentHealth -= damageAfterDefense;
        Debug.Log("Player took damage: " + damageAfterDefense + ", current health: " + currentHealth);

        PlaySound(hitSound); // 피격 시 효과음 재생
        PlayHitEffect(); // 피격 시 시각 효과 재생
        UpdateHealthBar(); // 체력바 업데이트

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;
        healthBarImage.fillAmount = healthRatio; // 체력바의 fillAmount 조정
    }

    void Die()
    {
        Debug.Log("Player died!");
        input.isDead = true;
        animator.SetTrigger("IsDie"); // 죽는 애니메이션 트리거 설정
        // Destroy(gameObject); // 게임 오브젝트 파괴는 애니메이션 이후에 수행
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
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1.0f); // 효과가 1초 후에 사라지도록 설정
        }
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
