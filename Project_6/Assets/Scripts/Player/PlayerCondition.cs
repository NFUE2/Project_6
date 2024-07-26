using UnityEngine;
using UnityEngine.UI;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public float maxHealth = 100f;
    private float currentHealth;

    public PlayerDataSO PlayerData; // PlayerDataSO �߰�
    public PlayerInput input;
    public Image healthBarImage; // ü�¹� �̹���
    private Animator animator; // Animator ������Ʈ �߰�
    public AudioClip hitSound; // �ǰ� �� ȿ���� �߰�
    private AudioSource audioSource; // AudioSource ������Ʈ �߰�
    public GameObject hitEffect; // �ǰ� �� �ð� ȿ�� �߰�

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ��������
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        float damageAfterDefense = Mathf.Max(damage - PlayerData.playerdefense, 0); // PlayerData�� ������ ������ ������ ���
        currentHealth -= damageAfterDefense;
        Debug.Log("Player took damage: " + damageAfterDefense + ", current health: " + currentHealth);

        PlaySound(hitSound); // �ǰ� �� ȿ���� ���
        PlayHitEffect(); // �ǰ� �� �ð� ȿ�� ���
        UpdateHealthBar(); // ü�¹� ������Ʈ

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;
        healthBarImage.fillAmount = healthRatio; // ü�¹��� fillAmount ����
    }

    void Die()
    {
        Debug.Log("Player died!");
        input.isDead = true;
        animator.SetTrigger("IsDie"); // �״� �ִϸ��̼� Ʈ���� ����
        // Destroy(gameObject); // ���� ������Ʈ �ı��� �ִϸ��̼� ���Ŀ� ����
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
            Destroy(effect, 1.0f); // ȿ���� 1�� �Ŀ� ��������� ����
        }
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
