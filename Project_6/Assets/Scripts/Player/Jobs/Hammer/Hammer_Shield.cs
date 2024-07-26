using UnityEngine;
using Photon.Pun;

public class Shield : MonoBehaviour, IDamagable
{
    public AudioClip hitSound; // 피격 시 효과음
    public GameObject hitEffect; // 피격 시 파티클 효과
    private AudioSource audioSource; // AudioSource 컴포넌트

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
    }

    public void TakeDamage(float damage)
    {
        PlayHitEffects(); // 피격 시 효과 재생
    }

    private void PlayHitEffects()
    {
        // 피격 시 효과음 재생
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // 피격 시 파티클 효과 생성
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1.0f); // 효과가 1초 후에 사라지도록 설정
        }
    }
}

