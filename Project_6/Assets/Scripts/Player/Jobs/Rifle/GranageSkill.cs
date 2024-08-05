using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeSkill : SkillBase
{
    public GameObject rifleGrenade; // ����ź ������
    public Transform attackPoint; // �÷��̾��� ���� ����
    public PlayerDataSO PlayerData; // �÷��̾� �����͸� �����ϴ� ScriptableObject

    public AudioClip fireSound; // �߻� ȿ����
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    void Start()
    {
        // �÷��̾� �����Ϳ��� ��ų�� ��ٿ� �ð��� �ʱ�ȭ�մϴ�.
        cooldownDuration = PlayerData.SkillECooldown;

        // ����� �ҽ� ������Ʈ �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        // ��ų�� ���� ��ٿ� ������ Ȯ���մϴ�.
        if (Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("��ų�� ��ٿ� ���Դϴ�.");
            return;
        }

        // ���콺 ��ġ�� ȭ�� ��ǥ���� ���� ��ǥ�� ��ȯ�մϴ�.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // ���� ������ ����մϴ� (���콺 ��ġ���� �÷��̾��� ���� ���������� ����).
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        // ����ź �������� �÷��̾��� ���� ������ �ν��Ͻ�ȭ�մϴ�.
        //GameObject grenadeInstance = Instantiate(rifleGrenade, attackPoint.position, Quaternion.identity);
        float angle = Mathf.Atan2(mousePosition.y,mousePosition.x) * Mathf.Rad2Deg;
        GameObject grenadeInstance = PhotonNetwork.Instantiate(rifleGrenade.name, attackPoint.position, Quaternion.Euler(0,0, angle));

        // ����ź�� ������ �����մϴ�.
        //grenadeInstance.GetComponent<Grenade>().SetDirection(attackDirection);

        // ����ź�� �ӵ��� �����մϴ�.
        //Rigidbody2D rb = grenadeInstance.GetComponent<Rigidbody2D>();
        //if (rb != null)
        //{
        //    rb.velocity = attackDirection * 10f;
        //}

        // �߻� ȿ���� ���
        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        // ������ �׼� �ð��� ���� �ð����� ������Ʈ�մϴ�.
        lastActionTime = Time.time;

        // ��ٿ� �ؽ�Ʈ�� ������Ʈ�մϴ�.
        UpdateCooldownText();
    }
}
