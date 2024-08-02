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

    private List<GameObject> targetMarkers = new List<GameObject>();
    private Camera mainCamera;

    void Start()
    {
        // �÷��̾� �����Ϳ��� ��ų�� ��ٿ� �ð��� �ʱ�ȭ�մϴ�.
        cooldownDuration = PlayerData.SkillQCooldown;
        mainCamera = Camera.main;
    }

    public override void UseSkill()
    {
        // ��ų�� ���� ��ٿ� ������ Ȯ���մϴ�.
        if (Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("��ų�� ��ٿ� ���Դϴ�.");
            return;
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
                for (int i = 0; i < 3; i++)
                {
                    Vector2 randomPoint = (Vector2)enemy.bounds.center + Random.insideUnitCircle * enemy.bounds.extents;
                    CreateTargetMarker(randomPoint, enemy.gameObject);
                }
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
    }

    private void ClearTargetMarkers()
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

    public void OnTargetClicked(GameObject target)
    {
        // ������ �������� �����ϴ�.
        IDamagable damagable = target.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(skillDamage); // �ν����Ϳ��� ������ ��ų �������� ����
        }

        // Ŭ���� ������ ����
        targetMarkers.RemoveAll(marker => marker == null || marker.GetComponent<TargetMarker>().target == target);
    }
}
