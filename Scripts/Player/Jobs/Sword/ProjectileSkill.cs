using UnityEngine;
using TMPro;
using Photon.Pun;

public class ProjectileSkill : SkillBase
{
    public GameObject projectilePrefab;
    public Transform attackPoint;
    public PlayerDataSO PlayerData;
    public AudioClip shootSound; // �߻� �� ȿ���� �߰�
    private AudioSource audioSource; // AudioSource ������Ʈ �߰�

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ��������
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // ����ü ����
        GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, attackPoint.position, Quaternion.Euler(0,0,angle));
        //projectile.transform.localEulerAngles = new Vector3(0, 0, angle);
        // projectile.GetComponent<Projectile>().SetDirection(dir); // ����ü ���� ����

        PlayShootSound(); // �߻� �� ȿ���� ���

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
