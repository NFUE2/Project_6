using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserSkill : SkillBase
{
    public float laserDuration = 2f;  // 레이저 지속 시간
    public float laserDamage = 10f;   // 레이저 피해량
    public float laserWidth = 0.1f;   // 레이저 기본 너비
    public float maxLaserWidth = 0.5f; // 레이저 최대 너비
    public LineRenderer laserRendererCore; // 하얀색 레이저 중심부를 그리기 위한 LineRenderer
    public LineRenderer laserRendererEdge; // 하늘색 레이저 테두리를 그리기 위한 LineRenderer
    public LayerMask enemyLayer;      // 적의 레이어
    public PlayerDataSO PlayerData;   // 플레이어 데이터
    public AudioClip laserSound;      // 레이저 효과음
    public GameObject impactEffectPrefab; // 충돌 시 생성할 파티클 효과 프리팹
    private AudioSource audioSource;  // 오디오 소스

    private List<GameObject> activeImpactEffects = new List<GameObject>();

    private void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown; // 스킬 E의 쿨다운 시간 설정

        // 오디오 소스 컴포넌트 가져오기 또는 추가하기
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // LineRenderer 설정
        if (laserRendererCore == null || laserRendererEdge == null)
        {
            return;
        }
        SetupLineRenderer(laserRendererCore, Color.white, Color.white);
        SetupLineRenderer(laserRendererEdge, new Color(0.0f, 0.75f, 1.0f), new Color(0.0f, 0.75f, 1.0f), true);
    }

    private void SetupLineRenderer(LineRenderer lineRenderer, Color startColor, Color endColor, bool isEdge = false)
    {
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.startColor = startColor; // 시작 색상
        lineRenderer.endColor = endColor;   // 끝 색상
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false; // 처음에는 비활성화

        // 끝부분 둥글게 설정
        lineRenderer.numCapVertices = 10; // 끝부분을 둥글게 만들기 위해 추가

        // LineRenderer에 사용할 Material 설정
        Material laserMaterial = new Material(Shader.Find("Unlit/Color"));
        laserMaterial.SetColor("_Color", startColor); // 색상 설정
        lineRenderer.material = laserMaterial;

        if (isEdge)
        {
            lineRenderer.startWidth = laserWidth + 0.05f; // 테두리는 중심부보다 약간 더 넓게 설정
            lineRenderer.endWidth = laserWidth + 0.05f;
        }
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return; // 쿨다운 시간이 지나지 않았으면 스킬 사용 불가

        lastActionTime = Time.time; // 마지막 스킬 사용 시간 갱신
        StartCoroutine(FireLaser()); // 레이저 발사 코루틴 시작
    }

    private IEnumerator FireLaser()
    {
        laserRendererCore.enabled = true; // 레이저 중심부 렌더러 활성화
        laserRendererEdge.enabled = true; // 레이저 테두리 렌더러 활성화

        // 레이저 효과음 재생
        if (laserSound != null)
        {
            audioSource.PlayOneShot(laserSound);
        }

        // 마우스 위치 기준 레이저 방향 설정
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0; // Z 좌표 고정
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = (worldMousePosition - transform.position).normalized;

        // 레이저 시작점과 끝점 계산
        Vector3 laserStartPoint = transform.position; // 플레이어의 위치를 레이저 시작점으로 설정
        Vector3 laserEndPoint = laserStartPoint + direction * 100; // 충분히 먼 거리로 설정

        // 레이저 위치 설정
        laserRendererCore.SetPosition(0, laserStartPoint);
        laserRendererCore.SetPosition(1, laserEndPoint);
        laserRendererEdge.SetPosition(0, laserStartPoint);
        laserRendererEdge.SetPosition(1, laserEndPoint);

        // 적과의 충돌 감지 및 데미지 처리
        Ray ray = new Ray(laserStartPoint, direction); // Ray의 시작점도 플레이어 위치로 설정
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, enemyLayer);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                GameObject enemy = hit.collider.gameObject;
                if (impactEffectPrefab != null)
                {
                    GameObject impactEffect = Instantiate(impactEffectPrefab, hit.point, Quaternion.identity);
                    activeImpactEffects.Add(impactEffect);
                    Destroy(impactEffect, 2f); // 2초 후에 파티클 효과 파괴
                }

                if (hit.transform.TryGetComponent(out IPunDamagable damagable))
                {
                    damagable.Damage(laserDamage); // 적에게 데미지 부여
                }
            }
        }

        yield return new WaitForSeconds(laserDuration);

        laserRendererCore.enabled = false; // 레이저 중심부 렌더러 비활성화
        laserRendererEdge.enabled = false; // 레이저 테두리 렌더러 비활성화

        // 남아있는 모든 파티클 효과 제거
        foreach (var effect in activeImpactEffects)
        {
            if (effect != null)
            {
                Destroy(effect);
            }
        }
        activeImpactEffects.Clear();
    }
}
