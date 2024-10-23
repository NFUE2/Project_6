using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserSkill : SkillBase
{
    public float laserDuration = 2f;  // ������ ���� �ð�
    public float laserDamage = 10f;   // ������ ���ط�
    public float laserWidth = 0.1f;   // ������ �⺻ �ʺ�
    public float maxLaserWidth = 0.5f; // ������ �ִ� �ʺ�
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

        // LineRenderer�� ����� Material ����
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
        laserRendererCore.enabled = true; // ������ �߽ɺ� ������ Ȱ��ȭ
        laserRendererEdge.enabled = true; // ������ �׵θ� ������ Ȱ��ȭ

        // ������ ȿ���� ���
        if (laserSound != null)
        {
            audioSource.PlayOneShot(laserSound);
        }

        // ���콺 ��ġ ���� ������ ���� ����
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0; // Z ��ǥ ����
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = (worldMousePosition - transform.position).normalized;

        // ������ �������� ���� ���
        Vector3 laserStartPoint = transform.position; // �÷��̾��� ��ġ�� ������ ���������� ����
        Vector3 laserEndPoint = laserStartPoint + direction * 100; // ����� �� �Ÿ��� ����

        // ������ ��ġ ����
        laserRendererCore.SetPosition(0, laserStartPoint);
        laserRendererCore.SetPosition(1, laserEndPoint);
        laserRendererEdge.SetPosition(0, laserStartPoint);
        laserRendererEdge.SetPosition(1, laserEndPoint);

        // ������ �浹 ���� �� ������ ó��
        Ray ray = new Ray(laserStartPoint, direction); // Ray�� �������� �÷��̾� ��ġ�� ����
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
                    Destroy(impactEffect, 2f); // 2�� �Ŀ� ��ƼŬ ȿ�� �ı�
                }

                if (hit.transform.TryGetComponent(out IPunDamagable damagable))
                {
                    damagable.Damage(laserDamage); // ������ ������ �ο�
                }
            }
        }

        yield return new WaitForSeconds(laserDuration);

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
}
