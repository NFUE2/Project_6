using Photon.Pun;
using UnityEngine;

public class BombArrowSkill : SkillBase
{
    public GameObject bombArrow;
    public float fireAngle;
    public PlayerDataSO PlayerData;
    public AudioClip fireSound; // 발사 효과음
    private AudioSource audioSource;

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;

        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        lastActionTime = Time.time;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float baseAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        for (int i = 0; i < 3; i++)
        {
            float angle = baseAngle + (i - 1) * fireAngle;
            GameObject go = PhotonNetwork.Instantiate(bombArrow.name, transform.position, Quaternion.Euler(0, 0, angle));

            // 폭탄 화살의 방향 설정
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            go.GetComponent<BombArrow>().SetDirection(direction);
        }

        // 발사 효과음 재생
        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
        else
        {
            Debug.LogError("fireSound 또는 audioSource가 할당되지 않았습니다.");
        }
    }
}
