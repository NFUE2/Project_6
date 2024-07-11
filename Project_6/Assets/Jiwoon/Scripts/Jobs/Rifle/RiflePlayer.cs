using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class RiflePlayer : PlayerBase
{
    [Header("Attack")]
    private bool isAttackCooldown = false;
    private int attackCount = 0;
    private float attackTime;
    private float lastAttackTime;
    private Camera mainCamera;
    public GameObject attackPrefab; // ����ü
    public Transform attackPoint;
    private float cooldownDuration = 0.5f;

    [Header("Skill Q")]
    public GameObject targetPrefab;
    public int qSkillMaxTargets = 3;
    public LayerMask enemyLayerMask;
    private List<GameObject> targetMarkers = new List<GameObject>();
    private int remainingChances = 3; // ���� ��ȸ ��

    [Header("Skill E")]
    public GameObject rifleGrenade;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public override void Attack()
    {
        if (isAttackCooldown) return;

        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        attackCount++;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // ���콺�� ��ġ��
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // ���콺�� ��ġ������ �������� ���� ���� ��ġ���� ���� => ���� ����
        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.
        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.

        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // ���� �ӵ� ���� �Ѵ�.

        if (attackCount >= 1)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown() //�߻�ӵ�
    {
        isAttackCooldown = true;
        attackCount = 0;
        yield return new WaitForSeconds(cooldownDuration);
        isAttackCooldown = false;
    }

    public override void UseSkillQ()
    {
        if (remainingChances <= 0) return;

        remainingChances--;

        // ���� ����� 3���� ���� ã��
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 10f, enemyLayerMask);
        var sortedEnemies = hitEnemies.OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).Take(qSkillMaxTargets);

        foreach (var enemy in sortedEnemies)
        {
            int markerCount = enemy.CompareTag("Boss") ? 3 : 1; // ������ 3���� ������, �Ϲ� ���� 1���� ������
            for (int i = 0; i < markerCount; i++)
            {
                Vector2 markerPosition = enemy.CompareTag("Boss") ? GetRandomPointInCollider(enemy) : enemy.transform.position;
                GameObject targetMarker = Instantiate(targetPrefab, markerPosition, Quaternion.identity);
                targetMarker.GetComponent<TargetMarker>().Initialize(enemy.gameObject, this);
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
        // ������ �������� ������ ���� �߰�
        enemy.GetComponent<Enemy>().TakeDamage(10);
        RemoveMarker();
    }

    public void TargetMarkerMissed()
    {
        RemoveMarker();
    }

    private void RemoveMarker()
    {
        // ������ �ϳ� ���� �� ���� ��ȸ ������Ʈ
        remainingChances--;
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
                Destroy(marker);
            }
        }
        targetMarkers.Clear();
    }
}

public class TargetMarker : MonoBehaviour
{
    private GameObject targetEnemy;
    private RiflePlayer player;

    public void Initialize(GameObject enemy, RiflePlayer player)
    {
        this.targetEnemy = enemy;
        this.player = player;
    }

    private void Update()
    {
        if (targetEnemy != null)
        {
            transform.position = targetEnemy.transform.position;
        }
    }

    private void OnMouseDown()
    {
        player.TargetMarkerClicked(targetEnemy);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (targetEnemy == null)
        {
            player.TargetMarkerMissed();
        }
    }
}
