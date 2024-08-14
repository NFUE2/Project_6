using UnityEngine;
using System.Collections;
using Photon.Pun;

public class DashSkill : SkillBase
{
    public float dashDistance = 5f;
    public float dashSpeed = 10f;
    public Transform playerTransform;
    public LayerMask enemyLayer;
    public PlayerDataSO PlayerData;
    public int damage = 10; // ������ �� �߰�
    public AudioClip dashSound; // ��� ȿ���� �߰�
    private AudioSource audioSource;

    private void Start()
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
        PlayDashSound();
        StartCoroutine(Dash());
    }

    private void PlayDashSound()
    {
        if (dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }
    }

    private IEnumerator Dash()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition;
        if (playerTransform.localScale.x == 1)
        {
            endPosition = transform.position + transform.right * -(dashDistance);
        }
        else
        {
            endPosition = transform.position + transform.right * dashDistance;
        }

        float startTime = Time.time;

        while (Time.time < startTime + (dashDistance / dashSpeed))
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (Time.time - startTime) * dashSpeed / dashDistance);
            yield return null;
        }

        transform.position = endPosition;
        DealDamageToEnemiesOnPath(startPosition, endPosition);
    }

    private void DealDamageToEnemiesOnPath(Vector3 startPosition, Vector3 endPosition)
    {
        // �ڽ��� �߽��� ��� ���۰� ���� �߰� �������� ����
        Vector2 boxCenter = (startPosition + endPosition) / 2;
        // �ڽ��� ũ�⸦ ��� �Ÿ���ŭ ��� �����ϰ� ����(�Ǵ� �ʺ�)�� �˳��ϰ� ����
        Vector2 boxSize = new Vector2(dashDistance, 1f); // 1f�� ����(�Ǵ� �ʺ�), �ʿ信 ���� ���� ����
        Vector2 direction = endPosition - startPosition;

        // BoxCastAll�� ���� ����
        RaycastHit2D[] hits = Physics2D.BoxCastAll(boxCenter, boxSize, 0f, direction, dashDistance, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out IPunDamagable m))
            {
                Debug.Log($"Dealing damage to: {hit.collider.name}");
                m.Damage(damage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = (transform.position + transform.position + transform.right * dashDistance) / 2;
        Vector2 boxSize = new Vector2(dashDistance, 1f); // 1f�� ����(�Ǵ� �ʺ�)
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }


}

