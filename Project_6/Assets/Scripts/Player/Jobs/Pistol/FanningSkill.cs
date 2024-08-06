using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.InputSystem;

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
            //float fireAngle = Random.Range(-3f, 3f);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float fireAngle = Random.Range(-3f, 3f) + Mathf.Atan2(mousePos.y,mousePos.x) * Mathf.Rad2Deg;

            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

            // �ణ�� ��ġ �������� �߰��Ͽ� ����ü�� ��ġ�� �ʵ��� �մϴ�.
            //Vector3 offset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            //GameObject go = Instantiate(attackPrefab, transform.position + offset, Quaternion.identity);
            //go.GetComponent<Projectile>().SetDirection(Quaternion.Euler(0, 0, fireAngle) * direction);
            GameObject go = PhotonNetwork.Instantiate(attackPrefab.name, transform.position,Quaternion.Euler(0, 0, fireAngle));
            // �߻� ȿ���� ���
            PlayFiringSound();

            Debug.Log($"Projectile {i + 1} fired at direction {direction}");

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
            Debug.Log("Firing sound played: " + firingSound.name);
        }
        else
        {
            Debug.LogError("firingSound �Ǵ� audioSource�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
