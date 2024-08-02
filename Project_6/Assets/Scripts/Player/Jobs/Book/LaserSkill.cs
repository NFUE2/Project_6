using UnityEngine;
using System.Collections;

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
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, Mathf.Infinity, enemyLayer); // 2D �������� �浹�� ��� ��ü ����

        laserRendererCore.SetPosition(0, transform.position); // ������ ������ ����
        laserRendererEdge.SetPosition(0, transform.position);
        Vector3 laserEndPoint = transform.position + direction * 100; // �⺻ ������ ���� ����

        if (hits.Length > 0)
        {
            laserEndPoint = hits[hits.Length - 1].point; // ������ �浹������ ������ ���� ����

            // �浹 ������ ��ƼŬ ȿ�� ����
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, laserEndPoint, Quaternion.identity);
            }
        }

        laserRendererCore.SetPosition(1, laserEndPoint); // ������ ���� ����
        laserRendererEdge.SetPosition(1, laserEndPoint);

        // ������ Ȯ�� ȿ��
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / laserExpandTime; // ������ Ȯ��
            float width = Mathf.Lerp(laserWidth, maxLaserWidth, t);
            laserRendererCore.startWidth = width;
            laserRendererCore.endWidth = width;
            laserRendererEdge.startWidth = width + 0.05f;
            laserRendererEdge.endWidth = width + 0.05f;
            yield return null; // �� ������ ���
        }

        // ������ ���� �ð� ���� ������ ��ġ ����
        while (Time.time - startTime < laserDuration)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    IDamagable damagable = hit.collider.GetComponent<IDamagable>();
                    if (damagable != null)
                    {
                        damagable.TakeDamage(laserDamage); // ������ ���� ����
                        Debug.Log($"�� {hit.collider.gameObject.name}���� {laserDamage}�� ���ظ� �������ϴ�.");

                        // �浹 ������ ��ƼŬ ȿ�� ����
                        if (impactEffectPrefab != null)
                        {
                            Instantiate(impactEffectPrefab, hit.point, Quaternion.identity);
                        }
                    }
                }
            }
            yield return null; // �� ������ ���
        }

        laserRendererCore.enabled = false; // ������ �߽ɺ� ������ ��Ȱ��ȭ
        laserRendererEdge.enabled = false; // ������ �׵θ� ������ ��Ȱ��ȭ
    }
}
