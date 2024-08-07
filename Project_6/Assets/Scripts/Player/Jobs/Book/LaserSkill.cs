using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserSkill : SkillBase
{
    public float laserDuration = 2f;  // ������ ���� �ð�
    public float laserDamage = 10f;   // ������ ���ط�
    public float laserWidth = 0.1f;   // ������ �⺻ �ʺ�
    public float maxLaserWidth = 0.5f; // ������ �ִ� �ʺ�
    public float laserExpandTime = 0.1f; // ������ Ȯ�� �ð�
    public LineRenderer laserRendererCore; // �Ͼ�� ������ �߽ɺθ� �׸��� ���� LineRenderer
    public LineRenderer laserRendererEdge; // �ϴû� ������ �׵θ��� �׸��� ���� LineRenderer
    public LayerMask enemyLayer;      // ���� ���̾�
    public PlayerDataSO PlayerData;   // �÷��̾� ������
    public AudioClip laserSound;      // ������ ȿ����
    public GameObject impactEffectPrefab; // �浹 �� ������ ��ƼŬ ȿ�� ������
    private AudioSource audioSource;  // ����� �ҽ�

    private List<GameObject> activeImpactEffects = new List<GameObject>();

    private void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown; // ��ų E�� ��ٿ� �ð� ����

        // ����� �ҽ� ������Ʈ �������� �Ǵ� �߰��ϱ�
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // LineRenderer ����
        if (laserRendererCore == null || laserRendererEdge == null)
        {
            Debug.LogError("LineRenderer�� �Ҵ���� �ʾҽ��ϴ�. �ν����Ϳ��� �����ϼ���.");
            return;
        }
        SetupLineRenderer(laserRendererCore, Color.white, Color.white);
        SetupLineRenderer(laserRendererEdge, new Color(0.0f, 0.75f, 1.0f), new Color(0.0f, 0.75f, 1.0f), true);
    }

    private void SetupLineRenderer(LineRenderer lineRenderer, Color startColor, Color endColor, bool isEdge = false)
    {
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.startColor = startColor; // ���� ����
        lineRenderer.endColor = endColor;   // �� ����
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false; // ó������ ��Ȱ��ȭ

        // ���κ� �ձ۰� ����
        lineRenderer.numCapVertices = 10; // ���κ��� �ձ۰� ����� ���� �߰�

        // �׵θ� ������ ���� ��Ƽ���� ����
        Material laserMaterial = new Material(Shader.Find("Unlit/Color"));
        laserMaterial.SetColor("_Color", startColor); // ���� ����
        lineRenderer.material = laserMaterial;

        if (isEdge)
        {
            lineRenderer.startWidth = laserWidth + 0.05f; // �׵θ��� �߽ɺκ��� �ణ �� �а� ����
            lineRenderer.endWidth = laserWidth + 0.05f;
        }
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return; // ��ٿ� �ð��� ������ �ʾ����� ��ų ��� �Ұ�

        lastActionTime = Time.time; // ������ ��ų ��� �ð� ����
        StartCoroutine(FireLaser()); // ������ �߻� �ڷ�ƾ ����
    }

    private IEnumerator FireLaser()
    {
        float startTime = Time.time; // ������ �߻� ���� �ð�
        laserRendererCore.enabled = true; // ������ �߽ɺ� ������ Ȱ��ȭ
        laserRendererEdge.enabled = true; // ������ �׵θ� ������ Ȱ��ȭ

        // ������ ȿ���� ���
        if (laserSound != null)
        {
            audioSource.PlayOneShot(laserSound);
            Debug.Log("������ ȿ���� ���: " + laserSound.name);
        }
        else
        {
            Debug.LogError("laserSound�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // ������ ���� ����
        Vector3 mousePosition = Input.mousePosition; // ���콺 ��ġ
        mousePosition.z = Camera.main.nearClipPlane; // ī�޶� ���� ��� ����
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition); // ���� ��ǥ�� ��ȯ
        Vector3 direction = (worldMousePosition - transform.position).normalized; // ������ ���� ���

        Ray ray = new Ray(transform.position, direction); // ������ ���� ����

        laserRendererCore.SetPosition(0, transform.position); // ������ ������ ����
        laserRendererEdge.SetPosition(0, transform.position);
        Vector3 laserEndPoint = transform.position + direction * 100; // �⺻ ������ ���� ����

        HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

        float t = 0;
        while (t < laserExpandTime)
        {
            t += Time.deltaTime;
            float width = Mathf.Lerp(laserWidth, maxLaserWidth, t / laserExpandTime);
            laserRendererCore.startWidth = width;
            laserRendererCore.endWidth = width;
            laserRendererEdge.startWidth = width + 0.05f;
            laserRendererEdge.endWidth = width + 0.05f;

            UpdateLaserPositions(ray, ref laserEndPoint, hitEnemies);

            yield return null;
        }

        // ������ ���� �ð� ���� ������ ��ġ ����
        while (Time.time - startTime < laserDuration)
        {
            // �������� �������� ���� ����
            UpdateLaserPositions(ray, ref laserEndPoint, hitEnemies, false);
            yield return null;
        }

        // ������ �پ��� ȿ��
        t = 0;
        while (t < laserExpandTime)
        {
            t += Time.deltaTime;
            float width = Mathf.Lerp(maxLaserWidth, laserWidth, t / laserExpandTime);
            laserRendererCore.startWidth = width;
            laserRendererCore.endWidth = width;
            laserRendererEdge.startWidth = width + 0.05f;
            laserRendererEdge.endWidth = width + 0.05f;

            UpdateLaserPositions(ray, ref laserEndPoint, hitEnemies, false);

            yield return null;
        }

        laserRendererCore.enabled = false; // ������ �߽ɺ� ������ ��Ȱ��ȭ
        laserRendererEdge.enabled = false; // ������ �׵θ� ������ ��Ȱ��ȭ

        // �����ִ� ��� ��ƼŬ ȿ�� ����
        foreach (var effect in activeImpactEffects)
        {
            if (effect != null)
            {
                Destroy(effect);
            }
        }
        activeImpactEffects.Clear();
    }

    private void UpdateLaserPositions(Ray ray, ref Vector3 laserEndPoint, HashSet<GameObject> hitEnemies, bool updateRayDirection = true)
    {
        if (updateRayDirection)
        {
            ray.direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, enemyLayer); // 2D �������� �浹�� ��� ��ü ����

        if (hits.Length > 0)
        {
            laserEndPoint = hits[hits.Length - 1].point; // ������ �浹������ ������ ���� ����

            // �浹 ������ ��ƼŬ ȿ�� ����
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    GameObject enemy = hit.collider.gameObject;
                    if (!hitEnemies.Contains(enemy))
                    {
                        hitEnemies.Add(enemy);

                        if (impactEffectPrefab != null)
                        {
                            GameObject impactEffect = Instantiate(impactEffectPrefab, hit.point, Quaternion.identity);
                            activeImpactEffects.Add(impactEffect);
                            Destroy(impactEffect, 2f); // 2�� �Ŀ� ��ƼŬ ȿ�� �ı�
                        }
                    }

                    if (hit.transform.TryGetComponent(out IDamagable damagable))
                    {
                        damagable.TakeDamage(laserDamage);
                    }
                }
            }
        }

        laserRendererCore.SetPosition(1, laserEndPoint); // ������ ���� ����
        laserRendererEdge.SetPosition(1, laserEndPoint);
    }
}
