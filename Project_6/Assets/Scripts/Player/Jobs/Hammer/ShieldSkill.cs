using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ShieldSkill : SkillBase
{
    public float ShieldDuration; // ���� ���� �ð�
    public GameObject shieldPrefab; // ���� ������
    public float shieldDistance = 1f; // ĳ���� �� ���� �Ÿ�
    private GameObject createdShield; // ������ ���и� ������ ����
    public PlayerDataSO PlayerData; // �÷��̾� �����͸� ������ ScriptableObject
    public AudioClip shieldSound; // ��� �� ȿ���� �߰�
    private AudioSource audioSource; // AudioSource ������Ʈ �߰�

    private Coroutine followCoroutine;
    private Coroutine destroyCoroutine;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown; // ��ٿ� �ð��� �÷��̾� �����Ϳ��� ������
        lastActionTime = -cooldownDuration; // lastActionTime�� �ʱ�ȭ�Ͽ� ó���� ��ٿ��� ������� �ʵ��� ��
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ��������
    }

    public override void UseSkill()
    {
        if (createdShield != null || !IsSkillReady()) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z���� 0���� �����Ͽ� 2D �������� ���

        // �÷��̾�� ���콺 ������ ���� ���
        Vector3 direction = (mousePosition - transform.position).normalized;

        // ���� ���� (�θ�-�ڽ� ���� ���� �������� ������Ʈ�� ����)
        createdShield = PhotonNetwork.Instantiate(shieldPrefab.name, transform.position, Quaternion.identity);

        // ������ �ڽ� �ݶ��̴� �߽��� �������� ���� ��ġ ����
        if (createdShield.TryGetComponent<BoxCollider2D>(out BoxCollider2D shieldCollider))
        {
            Vector3 shieldCenter = shieldCollider.bounds.center; // �ڽ� �ݶ��̴��� �߽�
            Vector3 offset = direction * shieldDistance;

            // ���� ��ġ�� �ݶ��̴� �߽ɿ� �°� ����
            createdShield.transform.position = transform.position + offset - (shieldCenter - createdShield.transform.position);
        }

        // ������ ȸ�� ���� (���콺 �������� ȸ��)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        createdShield.transform.rotation = Quaternion.Euler(0, 5, angle);

        // ���а� �÷��̾ ���� �̵��ϵ��� �ڷ�ƾ ����
        if (followCoroutine != null) StopCoroutine(followCoroutine);
        followCoroutine = StartCoroutine(FollowPlayer(direction));

        PlayShieldSound();

        lastActionTime = Time.time;

        if (destroyCoroutine != null) StopCoroutine(destroyCoroutine);
        destroyCoroutine = StartCoroutine(DestroyShieldAfterTime());
    }

    private IEnumerator FollowPlayer(Vector3 initialDirection)
    {
        while (createdShield != null)
        {
            // �÷��̾��� ���� ��ġ�� �������� ������ ��ġ�� ������Ʈ
            Vector3 newPosition = transform.position + initialDirection * shieldDistance;
            createdShield.transform.position = newPosition;

            yield return null; // �� �����Ӹ��� ������Ʈ
        }
    }

    private bool IsSkillReady()
    {
        return Time.time - lastActionTime >= cooldownDuration;
    }

    private IEnumerator DestroyShieldAfterTime()
    {
        yield return new WaitForSeconds(ShieldDuration);
        if (createdShield != null)
        {
            PhotonNetwork.Destroy(createdShield);
        }
    }

    private void PlayShieldSound()
    {
        if (shieldSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shieldSound);
        }
    }
}
