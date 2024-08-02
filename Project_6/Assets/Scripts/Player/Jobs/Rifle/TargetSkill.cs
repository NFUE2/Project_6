using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetSkill : SkillBase
{
    public GameObject targetMarkerPrefab; // 조준점 프리팹
    public float skillDamage = 10f; // 스킬 데미지, 인스펙터에서 조절 가능
    public PlayerDataSO PlayerData; // 플레이어 데이터를 포함하는 ScriptableObject
    public float targetingRange = 10f; // 타겟팅 범위
    public LayerMask enemyLayerMask; // 적의 레이어 마스크
    public int maxTargets = 3; // 최대 타겟 수

    private List<GameObject> targetMarkers = new List<GameObject>();
    private Camera mainCamera;

    void Start()
    {
        // 플레이어 데이터에서 스킬의 쿨다운 시간을 초기화합니다.
        cooldownDuration = PlayerData.SkillQCooldown;
        mainCamera = Camera.main;
    }

    public override void UseSkill()
    {
        // 스킬이 아직 쿨다운 중인지 확인합니다.
        if (Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("스킬이 쿨다운 중입니다.");
            return;
        }

        // 타겟팅 범위 내의 모든 적을 찾습니다.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyLayerMask);
        List<Collider2D> enemies = hitEnemies.OrderBy(h => Vector2.Distance(transform.position, h.transform.position)).Take(maxTargets).ToList();

        // 기존 조준점 제거
        ClearTargetMarkers();

        // 조준점 생성
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

        // 마지막 액션 시간을 현재 시간으로 업데이트합니다.
        lastActionTime = Time.time;

        // 쿨다운 텍스트를 업데이트합니다.
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
        // 적에게 데미지를 입힙니다.
        IDamagable damagable = target.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(skillDamage); // 인스펙터에서 설정된 스킬 데미지를 적용
        }

        // 클릭된 조준점 제거
        targetMarkers.RemoveAll(marker => marker == null || marker.GetComponent<TargetMarker>().target == target);
    }
}
