using UnityEngine;
using Photon.Pun;

public class Hammer_Shield : MonoBehaviourPun
{
    public AudioClip hitSound; // 피격 시 효과음
    public GameObject hitEffect; // 피격 시 파티클 효과
    private AudioSource audioSource; // AudioSource 컴포넌트

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
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

    // 충돌 감지 메서드 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            // 투사체 삭제
            Destroy(collision.gameObject);
            // 피격 효과 재생
            PlayHitEffects();
        }
    }
    public void SetParent(int index)
    {
        photonView.RPC(nameof(SetParentRPC), RpcTarget.All, index);
    }

    [PunRPC]
    public void SetParentRPC(int index)
    {
        transform.SetParent(GameManager.instance.players[index].transform);
    }
}
