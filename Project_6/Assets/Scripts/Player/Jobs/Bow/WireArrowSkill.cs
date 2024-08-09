using Photon.Pun;
using UnityEngine;

public class WireArrowSkill : SkillBase
{
    public GameObject wireArrow;
    public PlayerDataSO PlayerData;
    public AudioClip skillSound; // 스킬 사용 시 효과음
    private AudioSource audioSource;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
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

        PlaySkillSound(); // 스킬 사용 시 효과음 재생
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D 게임이므로 Z축을 0으로 설정
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject go = PhotonNetwork.Instantiate(wireArrow.name, transform.position, Quaternion.identity);

        Bow_WireArrow wireArrowComponent = go.GetComponent<Bow_WireArrow>();
        if (wireArrowComponent != null)
        {
            wireArrowComponent.player = transform;
            wireArrowComponent.SetDirection(direction);

            // 플레이어와 화살의 충돌을 무시하도록 설정
            Collider2D playerCollider = transform.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(go.GetComponent<Collider2D>(), playerCollider);
            }
        }
    }

    private void PlaySkillSound()
    {
        if (skillSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(skillSound);
        }
    }
}
