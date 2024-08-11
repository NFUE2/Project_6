using UnityEngine;
using Photon.Pun;
using System.Collections;

public class ShieldSkill : SkillBase
{
    public float ShieldDuration; // 방패 지속 시간
    public GameObject shieldPrefab; // 방패 프리팹
    public float shieldDistance = 1f; // 캐릭터 앞 방패 거리
    private GameObject createdShield; // 생성된 방패를 저장할 변수
    public PlayerDataSO PlayerData; // 플레이어 데이터를 저장할 ScriptableObject
    public AudioClip shieldSound; // 방어 시 효과음 추가
    private AudioSource audioSource; // AudioSource 컴포넌트 추가

    private Coroutine followCoroutine;
    private Coroutine destroyCoroutine;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown; // 쿨다운 시간을 플레이어 데이터에서 가져옴
        lastActionTime = -cooldownDuration; // lastActionTime을 초기화하여 처음에 쿨다운이 적용되지 않도록 함
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
    }

    public override void UseSkill()
    {
        if (createdShield != null || !IsSkillReady()) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition.x < transform.position.x) ? Vector3.left : Vector3.right;

        // 플레이어 앞에 방패 생성 위치 설정
        Vector3 shieldPosition = transform.position + direction * shieldDistance;

        // 정확한 위치에 방패 생성
        createdShield = PhotonNetwork.Instantiate(shieldPrefab.name, shieldPosition, Quaternion.identity);

        int index = GameManager.instance.players.IndexOf(gameObject);

        if (createdShield.TryGetComponent(out Hammer_Shield s))
        {
            s.SetParent(index);
            // 방패의 초기 위치를 다시 설정해준다.
            createdShield.transform.localPosition = direction * shieldDistance;
        }

        PlayShieldSound();

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
    //        // 마우스 위치를 기준으로 방패 생성 방향 결정
    //        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector3 direction = (mousePosition.x < transform.position.x) ? Vector3.left : Vector3.right;

    //        Vector3 shieldPosition = transform.position + direction * shieldDistance;
    //        createdShield.transform.position = shieldPosition;

    //        // 방패 방향 설정
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
