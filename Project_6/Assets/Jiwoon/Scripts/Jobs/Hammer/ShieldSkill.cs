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

    private Coroutine followCoroutine;
    private Coroutine destroyCoroutine;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown; // ��ٿ� �ð��� �÷��̾� �����Ϳ��� ������
        lastActionTime = -cooldownDuration; // lastActionTime�� �ʱ�ȭ�Ͽ� ó���� ��ٿ��� ������� �ʵ��� ��
    }

    public override void UseSkill()
    {
        if (createdShield != null || !IsSkillReady()) return;

        // ���콺 ��ġ�� �������� ���� ���� ���� ����
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition.x < transform.position.x) ? -transform.right : transform.right;

        Vector3 shieldPosition = transform.position + direction * shieldDistance;

        // �������� ���� ���� �ּ� ó��
        // createdShield = PhotonNetwork.Instantiate("Prototype/" + shieldPrefab.name, shieldPosition, Quaternion.identity);

        // ���ÿ��� ���� ����
        //createdShield = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity);
        createdShield = PhotonNetwork.Instantiate("Player/" + shieldPrefab.name,shieldPosition,Quaternion.identity);

        // ������ ȸ���� �ʱ�ȭ�Ͽ� ����
        createdShield.transform.rotation = Quaternion.identity;

        if (followCoroutine != null) StopCoroutine(followCoroutine);
        followCoroutine = StartCoroutine(FollowPlayer(direction));

        lastActionTime = Time.time;

        if (destroyCoroutine != null) StopCoroutine(destroyCoroutine);
        destroyCoroutine = StartCoroutine(DestroyShieldAfterTime());
    }

    private bool IsSkillReady()
    {
        return Time.time - lastActionTime >= cooldownDuration;
    }

    private IEnumerator FollowPlayer(Vector3 direction)
    {
        while (createdShield != null)
        {
            // ���а� �÷��̾ ����ٴ�
            createdShield.transform.position = transform.position + direction * shieldDistance;
            yield return null;
        }
    }

    private IEnumerator DestroyShieldAfterTime()
    {
        yield return new WaitForSeconds(ShieldDuration);
        if (createdShield != null)
        {
            //Destroy(createdShield);
            PhotonNetwork.Destroy(createdShield);
        }
    }
}
