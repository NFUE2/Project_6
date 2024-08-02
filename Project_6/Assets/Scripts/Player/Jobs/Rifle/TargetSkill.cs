using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TargetSkill : SkillBase
{
    public GameObject targetPrefab;
    public int qSkillMaxTargets = 3;
    public int skillQMaxChances = 5;
    public LayerMask enemyLayerMask;
    private List<GameObject> targetMarkers = new List<GameObject>();
    private int remainingChances;
    private RiflePlayer player;
    public PlayerDataSO PlayerData;
    public int damage;
    public float detectionRadius = 10f;

    public void SetPlayer(RiflePlayer player)
    {
        cooldownDuration = PlayerData.SkillQCooldown;
        remainingChances = skillQMaxChances;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
            return;
        }

        Debug.Log("UseSkill ȣ���");
        if (remainingChances <= 0) return;

        lastActionTime = Time.time;
        remainingChances--;

        Debug.Log("��ų ��� - �� ���� ����");
        Debug.Log($"Ž�� �ݰ�: {detectionRadius}, ���̾� ����ũ: {enemyLayerMask.value}");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayerMask);
        Debug.Log($"������ ���� ��: {hitEnemies.Length}");

        foreach (var enemy in hitEnemies)
        {
            Debug.Log($"������ ��: {enemy.gameObject.name}, ���̾�: {LayerMask.LayerToName(enemy.gameObject.layer)}");
            if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log($"{enemy.gameObject.name}��(��) Enemy ���̾ �ֽ��ϴ�.");
            }
            else
            {
                Debug.Log($"{enemy.gameObject.name}��(��) Enemy ���̾ ���� �ʽ��ϴ�.");
                continue;
            }
        }

        var sortedEnemies = hitEnemies.Where(e => e.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                                      .OrderBy(e => Vector2.Distance(transform.position, e.transform.position))
                                      .Take(qSkillMaxTargets);

        foreach (var enemy in sortedEnemies)
        {
            int markerCount = enemy.CompareTag("Boss") ? 3 : 1;
            Debug.Log($"{enemy.name} ������, Ÿ�� ��Ŀ ��: {markerCount}");
            for (int i = 0; i < markerCount; i++)
            {
                Vector2 markerPosition = enemy.CompareTag("Boss") ? GetRandomPointInCollider(enemy) : enemy.transform.position;
                GameObject targetMarker = Instantiate(targetPrefab, markerPosition, Quaternion.identity);
                targetMarker.GetComponent<TargetMarker>().Initialize(enemy.gameObject, player, this);
                targetMarkers.Add(targetMarker);
            }
        }
    }

    private Vector2 GetRandomPointInCollider(Collider2D collider)
    {
        Bounds bounds = collider.bounds;
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }

    public void TargetMarkerClicked(GameObject enemy)
    {
        IDamagable damagable = enemy.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(damage);
            Debug.Log($"�� {enemy.name}���� {damage}�� �������� �������ϴ�.");
        }
        RemoveMarker(enemy);
    }

    public void TargetMarkerMissed()
    {
        Debug.Log("Ÿ�� ��Ŀ ��ħ");
        ClearAllMarkers();
    }

    private void RemoveMarker(GameObject targetMarker)
    {
        targetMarkers.Remove(targetMarker);
        Destroy(targetMarker);
        if (targetMarkers.Count == 0)
        {
            remainingChances = 0;
        }
    }

    private void ClearAllMarkers()
    {
        foreach (var marker in targetMarkers)
        {
            if (marker != null)
            {
                Destroy(marker);
            }
        }
        targetMarkers.Clear();
        Debug.Log("��� ��Ŀ ���ŵ�");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
