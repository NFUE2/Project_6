using UnityEngine;
using Photon.Pun;

public class Hammer_Shield : MonoBehaviourPun
{
    public AudioClip hitSound; // �ǰ� �� ȿ����
    public GameObject hitEffect; // �ǰ� �� ��ƼŬ ȿ��
    private AudioSource audioSource; // AudioSource ������Ʈ

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ��������
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

    // �浹 ���� �޼��� �߰�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            // ����ü ����
            Destroy(collision.gameObject);
            // �ǰ� ȿ�� ���
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
