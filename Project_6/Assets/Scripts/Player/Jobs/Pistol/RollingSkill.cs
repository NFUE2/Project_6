using UnityEngine;
using System.Collections;

public class RollingSkill : SkillBase
{
    public PlayerDataSO PlayerData;
    [SerializeField] private float rollingDistance = 5f; // ������ �Ÿ�
    [SerializeField] private float rollingDuration = 0.5f; // ������ ���� �ð�
    [SerializeField] private AudioClip rollingSound; // �Ѹ� ȿ����

    private bool isRolling = false; // �Ѹ� ������ ����
    private bool isInvincible = false; // ���� �������� ����
    private AudioSource audioSource;
    private Vector3 targetPosition;
    private Rigidbody2D rb2d; // Rigidbody2D ������Ʈ

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;

        // ����� �ҽ� ������Ʈ �������� �Ǵ� �߰��ϱ�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Rigidbody2D ������Ʈ ��������
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    public override void UseSkill()
    {
        if (isRolling || Time.time - lastActionTime < cooldownDuration)
        {
            return;
        }
        StartCoroutine(Rolling());
    }

    private IEnumerator Rolling()
    {
        isRolling = true;
        isInvincible = true;
        lastActionTime = Time.time;

        // �ִϸ��̼� ���� (�ִϸ��̼� �ڵ� �߰� ����)
        // GetComponent<Animator>().SetTrigger("Roll");

        // �̵� ���� �� ��ǥ ��ġ ���
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Z�� �� ����
        Vector3 direction = (mousePosition - transform.position).normalized;

        // �¿� �������θ� ����
        direction = new Vector3(direction.x, 0, 0).normalized;

        targetPosition = transform.position + direction * rollingDistance;

        // �Ѹ� ȿ���� ���
        PlayRollingSound();

        // Rigidbody2D�� �߷��� Ȱ��ȭ
        rb2d.gravityScale = 1;

        // Ʈ�������� �̿��Ͽ� ��ǥ ��ġ�� �ε巴�� �̵�
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time < startTime + rollingDuration)
        {
            // ��ǥ ��ġ�� Ʈ������ �̵�
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, (Time.time - startTime) / rollingDuration);
            rb2d.MovePosition(newPosition); // MovePosition�� ����Ͽ� �ε巴�� �̵�

            yield return null;
        }

        // �Ѹ� ���� �� ��ġ�� ���� ��ǥ ��ġ�� ����
        rb2d.MovePosition(targetPosition);
        rb2d.gravityScale = 1; // �߷� ����

        isRolling = false;
        isInvincible = false;

        // �ִϸ��̼� ���� (�ִϸ��̼� �ڵ� �߰� ����)
        // GetComponent<Animator>().SetTrigger("StopRoll");
    }

    private void PlayRollingSound()
    {
        if (rollingSound != null)
        {
            audioSource.PlayOneShot(rollingSound);
        }
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
