using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TargetSkill : SkillBase
{
    public GameObject targetPrefab;
    public int qSkillMaxTargets = 3;
    public LayerMask enemyLayerMask;
    private List<GameObject> targetMarkers = new List<GameObject>();
    private int remainingChances;
    private RiflePlayer player;
    public PlayerDataSO PlayerData;

    public void SetPlayer(RiflePlayer player)
    {
        cooldownDuration = PlayerData.SkillQCooldown;
    }

    //오버라이드
    public override void UseSkill()
    {
        if (remainingChances <= 0) return;

        remainingChances--;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 10f, enemyLayerMask);
        var sortedEnemies = hitEnemies.OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).Take(qSkillMaxTargets);

        foreach (var enemy in sortedEnemies)
        {
            int markerCount = enemy.CompareTag("Boss") ? 3 : 1;
            for (int i = 0; i < markerCount; i++)
            {
                Vector2 markerPosition = enemy.CompareTag("Boss") ? GetRandomPointInCollider(enemy) : enemy.transform.position;
                GameObject targetMarker = Instantiate(targetPrefab, markerPosition, Quaternion.identity);
                targetMarker.GetComponent<TargetMarker>().Initialize(enemy.gameObject, player);
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
        Debug.Log($"데미지 적용: {enemy.name}");
        RemoveMarker();
    }

    public void TargetMarkerMissed()
    {
        Debug.Log("타겟 마커 놓침");
        RemoveMarker();
    }

    private void RemoveMarker()
    {
        remainingChances--;
        Debug.Log($"남은 기회: {remainingChances}");
        if (remainingChances <= 0)
        {
            ClearAllMarkers();
        }
    }

    private void ClearAllMarkers()
    {
        foreach (var marker in targetMarkers)
        {
            if (marker != null)
            {
                Destroy(marker); //비활성화 상태로 변경
                Debug.Log("마커 제거");
            }
        }
        targetMarkers.Clear();
        Debug.Log("모든 마커 제거됨");
    }
}
