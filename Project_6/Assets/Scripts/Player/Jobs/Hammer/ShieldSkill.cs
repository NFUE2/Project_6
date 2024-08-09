using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ShieldSkill : SkillBase
{
    public float ShieldDuration; // ���� ���� �ð�
    public GameObject shieldPrefab; // ���� ������
    public float shieldDistance = 1.0f; // ĳ���� �� ���� �Ÿ�
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

        // ���콺 ��ġ�� �������� ���� ���� ���� ����
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition.x < transform.position.x) ? Vector3.left : Vector3.right;

        Vector3 shieldPosition = transform.position + direction * shieldDistance;

        // ���ÿ��� ���� ����
        createdShield = PhotonNetwork.Instantiate(shieldPrefab.name, transform.position, Quaternion.identity);

        int index = GameManager.instance.players.IndexOf(gameObject);

        if (createdShield.TryGetComponent(out Hammer_Shield s))
            s.SetParent(index);

        // ���� ���� ����
        //if (direction == Vector3.left)
        //{
        //    createdShield.transform.localScale = new Vector3(-1, 1, 1);
        //}
        //else
        //{
        //    createdShield.transform.localScale = new Vector3(1, 1, 1);
        //}

        PlayShieldSound(); // ���� ���� �� ȿ���� ���

        //if (followCoroutine != null) StopCoroutine(followCoroutine);
        //followCoroutine = StartCoroutine(FollowPlayer());

        lastActionTime = Time.time;

        if (destroyCoroutine != null) StopCoroutine(destroyCoroutine);
        destroyCoroutine = StartCoroutine(DestroyShieldAfterTime());
    }

    private bool IsSkillReady()
    {
        return Time.time - lastActionTime >= cooldownDuration;
    }

    //private IEnumerator FollowPlayer()
    //{
    //    while (createdShield != null)
    //    {
    //        // ���콺 ��ġ�� �������� ���� ���� ���� ����
    //        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector3 direction = (mousePosition.x < transform.position.x) ? Vector3.left : Vector3.right;

    //        Vector3 shieldPosition = transform.position + direction * shieldDistance;
    //        createdShield.transform.position = shieldPosition;

    //        // ���� ���� ����
    //        if (direction == Vector3.left)
    //        {
    //            createdShield.transform.localScale = new Vector3(-1, 1, 1);
    //        }
    //        else
    //        {
    //            createdShield.transform.localScale = new Vector3(1, 1, 1);
    //        }

    //        yield return null;
    //    }
    //}

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
