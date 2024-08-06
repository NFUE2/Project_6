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

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;

        // ����� �ҽ� ������Ʈ �������� �Ǵ� �߰��ϱ�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void UseSkill()
    {
        if (isRolling || Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("RollingSkill: Cannot use skill, either on cooldown or already rolling.");
            return;
        }

        Debug.Log("RollingSkill: Skill used.");
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
        targetPosition = transform.position + direction * rollingDistance;

        // �Ѹ� ȿ���� ���
        PlayRollingSound();

        // Lerp�� ����Ͽ� �ε巴�� �̵�
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time < startTime + rollingDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (Time.time - startTime) / rollingDuration);
            yield return null;
        }

        transform.position = targetPosition; // ���� ��ġ ����
        Debug.Log("RollingSkill: Rolling ended.");

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
