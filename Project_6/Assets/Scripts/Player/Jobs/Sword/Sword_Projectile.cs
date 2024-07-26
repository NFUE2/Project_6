using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Projectile : MonoBehaviour
{
    public ProjectileDataSO data;
    public AudioClip hitSound; // ���� �� ȿ���� �߰�
    public GameObject hitEffect; // ���� �� ��ƼŬ ȿ�� �߰�
    private AudioSource audioSource; // AudioSource ������Ʈ �߰�

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ��������
    }

    private void Update()
    {
        transform.position += transform.right * data.moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerValue = data.target.value;
        int colLayer = collision.gameObject.layer;

        if (layerValue == 1 << colLayer && collision.TryGetComponent(out IDamagable target))
        {
            target.TakeDamage(data.damage);
            PlayHitEffects(collision.transform.position); // ���� �� ȿ�� ���
            Destroy(gameObject);
        }
    }

    private void PlayHitEffects(Vector3 hitPosition)
    {
        // ���� �� ȿ���� ���
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // ���� �� ��ƼŬ ȿ�� ����
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
            Destroy(effect, 1.0f); // ȿ���� 1�� �Ŀ� ��������� ����
        }
    }
}
