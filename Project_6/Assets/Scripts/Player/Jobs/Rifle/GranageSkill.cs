using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeSkill : SkillBase
{
    public GameObject rifleGrenade; // 수류탄 프리팹
    public Transform attackPoint; // 플레이어의 공격 지점
    public PlayerDataSO PlayerData; // 플레이어 데이터를 포함하는 ScriptableObject

    public AudioClip fireSound; // 발사 효과음
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    void Start()
    {
        // 플레이어 데이터에서 스킬의 쿨다운 시간을 초기화합니다.
        cooldownDuration = PlayerData.SkillECooldown;

        // 오디오 소스 컴포넌트 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        // 스킬이 아직 쿨다운 중인지 확인합니다.
        if (Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("스킬이 쿨다운 중입니다.");
            return;
        }

        // 마우스 위치를 화면 좌표에서 월드 좌표로 변환합니다.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // 공격 방향을 계산합니다 (마우스 위치에서 플레이어의 공격 지점까지의 방향).
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        // 수류탄 프리팹을 플레이어의 공격 지점에 인스턴스화합니다.
        //GameObject grenadeInstance = Instantiate(rifleGrenade, attackPoint.position, Quaternion.identity);
        float angle = Mathf.Atan2(mousePosition.y,mousePosition.x) * Mathf.Rad2Deg;
        GameObject grenadeInstance = PhotonNetwork.Instantiate(rifleGrenade.name, attackPoint.position, Quaternion.Euler(0,0, angle));

        // 수류탄의 방향을 설정합니다.
        //grenadeInstance.GetComponent<Grenade>().SetDirection(attackDirection);

        // 수류탄의 속도를 설정합니다.
        //Rigidbody2D rb = grenadeInstance.GetComponent<Rigidbody2D>();
        //if (rb != null)
        //{
        //    rb.velocity = attackDirection * 10f;
        //}

        // 발사 효과음 재생
        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        // 마지막 액션 시간을 현재 시간으로 업데이트합니다.
        lastActionTime = Time.time;

        // 쿨다운 텍스트를 업데이트합니다.
        UpdateCooldownText();
    }
}
