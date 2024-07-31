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
        Debug.Log("Wire Arrow Skill 사용됨");

        PlaySkillSound(); // 스킬 사용 시 효과음 재생

        GameObject go = Instantiate(wireArrow, transform.position, Quaternion.identity);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D 게임이므로 Z축을 0으로 설정
        Vector2 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        go.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Bow_WireArrow wireArrowComponent = go.GetComponent<Bow_WireArrow>();
        if (wireArrowComponent != null)
        {
            wireArrowComponent.player = transform;
        }
        else
        {
            Debug.LogError("Bow_WireArrow 컴포넌트를 찾을 수 없습니다.");
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
