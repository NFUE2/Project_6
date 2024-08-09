using UnityEngine;
using TMPro;
using Photon.Pun;

public class ProjectileSkill : SkillBase
{
    public GameObject projectilePrefab;
    public Transform attackPoint;
    public PlayerDataSO PlayerData;
    public AudioClip shootSound; // 발사 시 효과음 추가
    private AudioSource audioSource; // AudioSource 컴포넌트 추가

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트 가져오기
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 투사체 생성
        GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, attackPoint.position, Quaternion.Euler(0,0,angle));
        //projectile.transform.localEulerAngles = new Vector3(0, 0, angle);
        // projectile.GetComponent<Projectile>().SetDirection(dir); // 투사체 방향 설정

        PlayShootSound(); // 발사 시 효과음 재생

        lastActionTime = Time.time;
    }

    private void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
