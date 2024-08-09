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
    public AudioClip hitSound; // 타겟을 맞췄을 때의 효과음
    public AudioClip useSkillSound; // 스킬 사용 시 효과음
    public AudioClip missSound; // 타겟이 아닌 곳 클릭 시 효과음
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    private List<GameObject> targetMarkers = new List<GameObject>();
    private Camera mainCamera;
    private RiflePlayer riflePlayer; // RiflePlayer 참조

    void Start()
    {
        // 플레이어 데이터에서 스킬의 쿨다운 시간을 초기화합니다.
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
        // 스킬이 아직 쿨다운 중인지 확인합니다.
        if (Time.time - lastActionTime < cooldownDuration)
        {
            Debug.Log("스킬이 쿨다운 중입니다.");
            return;
        }

        // 스킬 사용 시 효과음 재생
        if (useSkillSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(useSkillSound);
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
                CreateBossTargetMarkers(enemy);
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

        // 타겟팅 모드 활성화
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

        // 중심에 첫 번째 타겟 마커 생성
        CreateTargetMarker(center, bossCollider.gameObject);

        // 중심에서 일정 거리만큼 떨어진 위치에 나머지 두 개의 타겟 마커 생성
        float distance = Mathf.Min(bossCollider.bounds.size.x, bossCollider.bounds.size.y) * 0.5f; // 보스 콜라이더 크기의 50% 거리로 설정
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
        // 적에게 데미지를 입힙니다.
        //if (target.TryGetComponent(out IDamagable m))
        if (target.TryGetComponent(out IPunDamagable m))
        {
            //m.TakeDamage(skillDamage);
            m.Damage(skillDamage);
        }

        // 효과음 재생
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // 클릭된 조준점 제거
        var markersToRemove = targetMarkers.Where(marker => marker != null && marker.GetComponent<TargetMarker>().target == target).ToList();
        foreach (var marker in markersToRemove)
        {
            targetMarkers.Remove(marker);
            Destroy(marker);
        }

        // 타겟 마커가 모두 제거되었는지 확인하지 않고 타겟팅 모드 유지
        if (targetMarkers.Count == 0)
        {
            riflePlayer.SetTargeting(true);
        }
    }

    public void OnMissClick()
    {
        // 효과음 재생
        if (missSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(missSound);
        }

        // 타겟 마커 제거 및 타겟팅 모드 비활성화
        ClearTargetMarkers();
        riflePlayer.SetTargeting(false);
    }
}
