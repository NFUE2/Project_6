using System.Collections;
using UnityEngine;

public class Sword_Projectile : MonoBehaviour
{
    public ProjectileDataSO data;
    public AudioClip hitSound; // 적중 시 효과음 추가
    public GameObject hitEffect; // 적중 시 파티클 효과 추가
    private AudioSource audioSource; // AudioSource 컴포넌트 추가

    private void Start()
    {
        // AudioSource 컴포넌트가 있는지 확인하고 없으면 추가
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

            Debug.Log("Hit detected! Playing hit effects."); // 디버그 로그 추가
            PlayHitEffects(collision.transform.position); // 적중 시 효과 재생
            Destroy(gameObject);
        }
    }

    private void PlayHitEffects(Vector3 hitPosition)
    {
        // 적중 시 효과음 재생
        if (hitSound != null && audioSource != null)
        {
            Debug.Log("Playing hit sound."); // 디버그 로그 추가
            audioSource.PlayOneShot(hitSound);
        }
        else
        {
            Debug.LogWarning("Hit sound or audio source is null.");
        }

        // 적중 시 파티클 효과 생성
        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
            Destroy(effect, 1.0f); // 효과가 1초 후에 사라지도록 설정
        }
    }
}


