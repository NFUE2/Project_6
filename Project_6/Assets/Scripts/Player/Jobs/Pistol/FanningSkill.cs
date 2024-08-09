using UnityEngine;
using System.Collections;
using Photon.Pun;

public class FanningSkill : SkillBase
{
    public Transform attackPoint;
    public GameObject attackPrefab;
    public bool IsFanningReady { get; private set; }
    public PlayerDataSO PlayerData;
    public AudioClip firingSound; // �߻� ȿ����
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;

        // ����� �ҽ� ������Ʈ �������� �Ǵ� �߰��ϱ�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        if (IsFanningReady || Time.time - lastActionTime < cooldownDuration) return;

        IsFanningReady = true;
        StartCoroutine(Fanning());
    }

    private IEnumerator Fanning()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;

        for (int i = 0; i < 6; i++)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 fireDirection = (mousePos - (Vector2)transform.position).normalized;
            float fireAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg + Random.Range(-3f, 3f);

            GameObject go = PhotonNetwork.Instantiate(attackPrefab.name, attackPoint.position, Quaternion.Euler(0, 0, fireAngle));
            Projectile projectile = go.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetDirection(fireDirection);
            }

            // �߻� ȿ���� ���
            PlayFiringSound();

            yield return new WaitForSeconds(0.1f);
        }

        IsFanningReady = false;
        lastActionTime = Time.time;
    }

    private void PlayFiringSound()
    {
        if (firingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(firingSound);
        }
    }
}
