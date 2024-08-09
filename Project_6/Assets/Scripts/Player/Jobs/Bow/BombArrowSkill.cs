using Photon.Pun;
using UnityEngine;

public class BombArrowSkill : SkillBase
{
    public GameObject bombArrow;
    public float fireAngle;
    public PlayerDataSO PlayerData;
    public AudioClip fireSound; // �߻� ȿ����
    private AudioSource audioSource;

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;

        // AudioSource ������Ʈ ��������
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

            // ��ź ȭ���� ���� ����
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            go.GetComponent<BombArrow>().SetDirection(direction);
        }

        // �߻� ȿ���� ���
        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
        else
        {
            Debug.LogError("fireSound �Ǵ� audioSource�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
