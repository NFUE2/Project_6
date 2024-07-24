using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ShieldSkill : SkillBase
{
    public float ShieldDuration; // 방패 지속 시간
    public GameObject shieldPrefab; // 방패 프리팹
    public float shieldDistance = 1.0f; // 캐릭터 앞 방패 거리
    private GameObject createdShield; // 생성된 방패를 저장할 변수
    public PlayerDataSO PlayerData; // 플레이어 데이터를 저장할 ScriptableObject

    private Coroutine followCoroutine;
    private Coroutine destroyCoroutine;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown; // 쿨다운 시간을 플레이어 데이터에서 가져옴
        lastActionTime = -cooldownDuration; // lastActionTime을 초기화하여 처음에 쿨다운이 적용되지 않도록 함
    }

    public override void UseSkill()
    {
        if (createdShield != null || !IsSkillReady()) return;

        // 마우스 위치를 기준으로 방패 생성 방향 결정
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition.x < transform.position.x) ? -transform.right : transform.right;

        Vector3 shieldPosition = transform.position + direction * shieldDistance;

        // 서버에서 방패 생성 주석 처리
        // createdShield = PhotonNetwork.Instantiate("Prototype/" + shieldPrefab.name, shieldPosition, Quaternion.identity);

        // 로컬에서 방패 생성
        //createdShield = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity);
        createdShield = PhotonNetwork.Instantiate("Player/" + shieldPrefab.name,shieldPosition,Quaternion.identity);

        // 방패의 회전을 초기화하여 고정
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
            // 방패가 플레이어를 따라다님
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
