using UnityEngine;
using System.Collections;

public class LaserSkill : SkillBase
{
    public float laserDuration = 2f;  // 레이저 지속 시간
    public float laserDamage = 10f;   // 레이저 피해량
    public float laserWidth = 0.1f;   // 레이저 기본 너비
    public float maxLaserWidth = 0.5f; // 레이저 최대 너비
    public float laserExpandTime = 0.1f; // 레이저 확장 시간
    public LineRenderer laserRendererCore; // 하얀색 레이저 중심부를 그리기 위한 LineRenderer
    public LineRenderer laserRendererEdge; // 하늘색 레이저 테두리를 그리기 위한 LineRenderer
    public LayerMask enemyLayer;      // 적의 레이어
    public PlayerDataSO PlayerData;   // 플레이어 데이터
    public AudioClip laserSound;      // 레이저 효과음
    public GameObject impactEffectPrefab; // 충돌 시 생성할 파티클 효과 프리팹
    private AudioSource audioSource;  // 오디오 소스

    private void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown; // 스킬 E의 쿨다운 시간 설정

        // 오디오 소스 컴포넌트 가져오기 또는 추가하기
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // LineRenderer 설정
        if (laserRendererCore == null || laserRendererEdge == null)
        {
            Debug.LogError("LineRenderer가 할당되지 않았습니다. 인스펙터에서 설정하세요.");
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

        // 테두리 설정을 위한 머티리얼 설정
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
        float startTime = Time.time; // 레이저 발사 시작 시간
        laserRendererCore.enabled = true; // 레이저 중심부 렌더러 활성화
        laserRendererEdge.enabled = true; // 레이저 테두리 렌더러 활성화

        // 레이저 효과음 재생
        if (laserSound != null)
        {
            audioSource.PlayOneShot(laserSound);
            Debug.Log("레이저 효과음 재생: " + laserSound.name);
        }
        else
        {
            Debug.LogError("laserSound가 할당되지 않았습니다.");
        }

        // 레이저 방향 설정
        Vector3 mousePosition = Input.mousePosition; // 마우스 위치
        mousePosition.z = Camera.main.nearClipPlane; // 카메라 근접 평면 설정
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition); // 월드 좌표로 변환
        Vector3 direction = (worldMousePosition - transform.position).normalized; // 레이저 방향 계산

        Ray ray = new Ray(transform.position, direction); // 레이저 레이 생성
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, Mathf.Infinity, enemyLayer); // 2D 레이저와 충돌한 모든 객체 감지

        laserRendererCore.SetPosition(0, transform.position); // 레이저 시작점 설정
        laserRendererEdge.SetPosition(0, transform.position);
        Vector3 laserEndPoint = transform.position + direction * 100; // 기본 레이저 끝점 설정

        if (hits.Length > 0)
        {
            laserEndPoint = hits[hits.Length - 1].point; // 마지막 충돌점으로 레이저 끝점 설정

            // 충돌 지점에 파티클 효과 생성
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, laserEndPoint, Quaternion.identity);
            }
        }

        laserRendererCore.SetPosition(1, laserEndPoint); // 레이저 끝점 설정
        laserRendererEdge.SetPosition(1, laserEndPoint);

        // 레이저 확장 효과
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / laserExpandTime; // 빠르게 확장
            float width = Mathf.Lerp(laserWidth, maxLaserWidth, t);
            laserRendererCore.startWidth = width;
            laserRendererCore.endWidth = width;
            laserRendererEdge.startWidth = width + 0.05f;
            laserRendererEdge.endWidth = width + 0.05f;
            yield return null; // 한 프레임 대기
        }

        // 레이저 지속 시간 동안 고정된 위치 유지
        while (Time.time - startTime < laserDuration)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    IDamagable damagable = hit.collider.GetComponent<IDamagable>();
                    if (damagable != null)
                    {
                        damagable.TakeDamage(laserDamage); // 적에게 피해 적용
                        Debug.Log($"적 {hit.collider.gameObject.name}에게 {laserDamage}의 피해를 입혔습니다.");

                        // 충돌 지점에 파티클 효과 생성
                        if (impactEffectPrefab != null)
                        {
                            Instantiate(impactEffectPrefab, hit.point, Quaternion.identity);
                        }
                    }
                }
            }
            yield return null; // 한 프레임 대기
        }

        laserRendererCore.enabled = false; // 레이저 중심부 렌더러 비활성화
        laserRendererEdge.enabled = false; // 레이저 테두리 렌더러 비활성화
    }
}
