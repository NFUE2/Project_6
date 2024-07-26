using UnityEngine;
using Photon.Pun;

public class Shield : MonoBehaviour, IDamagable
{
    public AudioClip hitSound; // �ǰ� �� ȿ����
    public GameObject hitEffect; // �ǰ� �� ��ƼŬ ȿ��
    private AudioSource audioSource; // AudioSource ������Ʈ

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ��������
    }

    public void TakeDamage(float damage)
    {
        PlayHitEffects(); // �ǰ� �� ȿ�� ���
    }

    private void PlayHitEffects()
    {
        // �ǰ� �� ȿ���� ���
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // �ǰ� �� ��ƼŬ ȿ�� ����
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1.0f); // ȿ���� 1�� �Ŀ� ��������� ����
        }
    }
}

