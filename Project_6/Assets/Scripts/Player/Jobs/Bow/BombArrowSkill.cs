using UnityEngine;
// using Photon.Pun; // ���� ���� �κ� �ּ� ó��

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
            // ���� ���� �κ� �ּ� ó��
            // GameObject go = PhotonNetwork.Instantiate("Prototype/" + bombArrow.name, transform.position, Quaternion.identity);
            GameObject go = Instantiate(bombArrow, transform.position, Quaternion.identity);
            float angle = baseAngle + (i - 1) * fireAngle;
            go.transform.rotation = Quaternion.Euler(0, 0, angle);

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
