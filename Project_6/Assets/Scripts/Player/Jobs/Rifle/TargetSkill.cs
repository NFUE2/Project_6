using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetSkill : SkillBase
{
    public GameObject targetMarkerPrefab; // ������ ������
    public float skillDamage = 10f; // ��ų ������, �ν����Ϳ��� ���� ����
    public PlayerDataSO PlayerData; // �÷��̾� �����͸� �����ϴ� ScriptableObject
    public float targetingRange = 10f; // Ÿ���� ����
    public LayerMask enemyLayerMask; // ���� ���̾� ����ũ
    public int maxTargets = 3; // �ִ� Ÿ�� ��
    public AudioClip hitSound; // Ÿ���� ������ ���� ȿ����
    public AudioClip useSkillSound; // ��ų ��� �� ȿ����
    public AudioClip missSound; // Ÿ���� �ƴ� �� Ŭ�� �� ȿ����
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    private List<GameObject> targetMarkers = new List<GameObject>();
    private Camera mainCamera;
    private RiflePlayer riflePlayer; // RiflePlayer ����

    void Start()
    {
        // �÷��̾� �����Ϳ��� ��ų�� ��ٿ� �ð��� �ʱ�ȭ�մϴ�.
        cooldownDuration = PlayerData.SkillQCooldown;
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Initialize(RiflePlayer player)
    {
        riflePlayer = player;
    }

    public override void UseSkill()
    {
        // ��ų�� ���� ��ٿ� ������ Ȯ���մϴ�.
        if (Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("��ų�� ��ٿ� ���Դϴ�.");
            return;
        }

        // ��ų ��� �� ȿ���� ���
        if (useSkillSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(useSkillSound);
        }

        // Ÿ���� ���� ���� ��� ���� ã���ϴ�.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyLayerMask);
        List<Collider2D> enemies = hitEnemies.OrderBy(h => Vector2.Distance(transform.position, h.transform.position)).Take(maxTargets).ToList();

        // ���� ������ ����
        ClearTargetMarkers();

        // ������ ����
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Boss"))
            {
                CreateBossTargetMarkers(enemy);
            }
            else
            {
                CreateTargetMarker(enemy.bounds.center, enemy.gameObject);
            }
        }

        // ������ �׼� �ð��� ���� �ð����� ������Ʈ�մϴ�.
        lastActionTime = Time.time;

        // ��ٿ� �ؽ�Ʈ�� ������Ʈ�մϴ�.
        UpdateCooldownText();

        // Ÿ���� ��� Ȱ��ȭ
        riflePlayer.SetTargeting(true);
    }

    public void ClearTargetMarkers()
    {
        foreach (GameObject marker in targetMarkers)
        {
            Destroy(marker);
        }
        targetMarkers.Clear();
    }

    private void CreateTargetMarker(Vector2 position, GameObject target)
    {
        GameObject markerInstance = Instantiate(targetMarkerPrefab, position, Quaternion.identity);
        TargetMarker marker = markerInstance.GetComponent<TargetMarker>();
        if (marker != null)
        {
            marker.Initialize(target, this);
        }
        targetMarkers.Add(markerInstance);
    }

    private void CreateBossTargetMarkers(Collider2D bossCollider)
    {
        Vector2 center = bossCollider.bounds.center;

        // �߽ɿ� ù ��° Ÿ�� ��Ŀ ����
        CreateTargetMarker(center, bossCollider.gameObject);

        // �߽ɿ��� ���� �Ÿ���ŭ ������ ��ġ�� ������ �� ���� Ÿ�� ��Ŀ ����
        float distance = Mathf.Min(bossCollider.bounds.size.x, bossCollider.bounds.size.y) * 0.5f; // ���� �ݶ��̴� ũ���� 50% �Ÿ��� ����
        Vector2[] offsets = {
            new Vector2(distance, distance),
            new Vector2(-distance, -distance)
        };

        foreach (var offset in offsets)
        {
            Vector2 newPosition = center + offset;
            CreateTargetMarker(newPosition, bossCollider.gameObject);
        }
    }

    public void OnTargetClicked(GameObject target)
    {
        // ������ �������� �����ϴ�.
        //if (target.TryGetComponent(out IDamagable m))
        if (target.TryGetComponent(out IPunDamagable m))
        {
            //m.TakeDamage(skillDamage);
            m.Damage(skillDamage);
        }

        // ȿ���� ���
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // Ŭ���� ������ ����
        var markersToRemove = targetMarkers.Where(marker => marker != null && marker.GetComponent<TargetMarker>().target == target).ToList();
        foreach (var marker in markersToRemove)
        {
            targetMarkers.Remove(marker);
            Destroy(marker);
        }

        // Ÿ�� ��Ŀ�� ��� ���ŵǾ����� Ȯ������ �ʰ� Ÿ���� ��� ����
        if (targetMarkers.Count == 0)
        {
            riflePlayer.SetTargeting(true);
        }
    }

    public void OnMissClick()
    {
        // ȿ���� ���
        if (missSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(missSound);
        }

        // Ÿ�� ��Ŀ ���� �� Ÿ���� ��� ��Ȱ��ȭ
        ClearTargetMarkers();
        riflePlayer.SetTargeting(false);
    }
}
