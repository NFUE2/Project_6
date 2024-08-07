using System.Collections;
using UnityEngine;

public class Sword_Projectile : MonoBehaviour
{
    public ProjectileDataSO data;
    public AudioClip hitSound; // ���� �� ȿ���� �߰�
    public GameObject hitEffect; // ���� �� ��ƼŬ ȿ�� �߰�
    private AudioSource audioSource; // AudioSource ������Ʈ �߰�

    private void Start()
    {
        // AudioSource ������Ʈ�� �ִ��� Ȯ���ϰ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        transform.position += transform.right * data.moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerValue = data.target.value;
        int colLayer = collision.gameObject.layer;

        if (layerValue == (1 << colLayer) && collision.TryGetComponent(out IPunDamagable target))
        {
            //target.TakeDamage(data.damage);
            target.Damage(data.damage);

            Debug.Log("Hit detected! Playing hit effects."); // ����� �α� �߰�
            PlayHitEffects(collision.transform.position); // ���� �� ȿ�� ���
            Destroy(gameObject);
        }
    }

    private void PlayHitEffects(Vector3 hitPosition)
    {
        // ���� �� ȿ���� ���
        if (hitSound != null && audioSource != null)
        {
            Debug.Log("Playing hit sound."); // ����� �α� �߰�
            audioSource.PlayOneShot(hitSound);
        }
        else
        {
            Debug.LogWarning("Hit sound or audio source is null.");
        }

        // ���� �� ��ƼŬ ȿ�� ����
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
            Destroy(effect, 1.0f); // ȿ���� 1�� �Ŀ� ��������� ����
        }
    }
}


