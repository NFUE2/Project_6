using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class RiflePlayer : PlayerBase
{
    public PlayerData PlayerData;

    [Header("Attack")]
    private bool isAttackCooldown = false;
    private int attackCount = 0;

    //공격부분 - 상위클래스로 이동
    private float attackTime;
    private float lastAttackTime;
    //====================================

    private Camera mainCamera;

    //원거리 캐릭터 클래스에서 구현
    public GameObject attackPrefab; // 투사체
    public Transform attackPoint;
    //======================================

    private float cooldownDuration = 0.5f; //? lastAttackTime랑 다른게 뭔지 모르겠어요

    //스킬클래스로 이동 - 만약 스킬클래스에서 처리 못하면 말해주세요
    [Header("Skill Q")]
    public GameObject targetPrefab;
    public int qSkillMaxTargets = 3;
    public LayerMask enemyLayerMask;
    private List<GameObject> targetMarkers = new List<GameObject>();
    private int remainingChances = 3; // 남은 기회 수

    [Header("Skill E")]
    public GameObject rifleGrenade;
    //====================================

    private void Start()
    {
        mainCamera = Camera.main;
    }

    //원거리 캐릭터에서 구현
    public override void Attack()
    {
        if (isAttackCooldown) return;

        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        attackCount++;

        //이건 input에서 할 수 있도록 해주세요, 이벤트 방식을 사용하면됩니다
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // 마우스의 위치값


        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // 마우스의 위치값에서 지정해준 공격 시작 위치값을 뺀다 => 공격 방향
        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // 총알을 생성해 발사해 공격한다.
        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity); // 총알을 생성해 발사해 공격한다.

        //투사체 클래스에서 구현
        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // 공격 속도 설정 한다.


        //볼트액션이라 1일때 작동하는 쿨타임인듯,lastAttackTime의 의도랑 같은것 같습니다, 제거
        if (attackCount >= 1) 
        {
            StartCoroutine(AttackCooldown());
        }
        //=========================
    }
    
    //제거
    private IEnumerator AttackCooldown() //발사속도
    {
        isAttackCooldown = true;
        attackCount = 0;
        yield return new WaitForSeconds(cooldownDuration);
        isAttackCooldown = false;
    }
    //===============================


    //스킬클래스에서 구현
    public override void UseSkillQ()
    {
        if (remainingChances <= 0) return;

        remainingChances--;

        // 가장 가까운 3개의 적을 찾기
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 10f, enemyLayerMask);
        var sortedEnemies = hitEnemies.OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).Take(qSkillMaxTargets);

        foreach (var enemy in sortedEnemies)
        {
            int markerCount = enemy.CompareTag("Boss") ? 3 : 1; // 보스는 3개의 조준점, 일반 적은 1개의 조준점
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
        // 적에게 데미지를 입히는 로직 추가
        // enemy.GetComponent<Enemy>().TakeDamage(10);
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
        // 조준점 하나 제거 및 남은 기회 업데이트
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
                Destroy(marker);
                Debug.Log("마커 제거");
            }
        }
        targetMarkers.Clear();
        Debug.Log("모든 마커 제거됨");
    }
    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < PlayerData.SkillECooldown) return;

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        GameObject grenadeInstance = Instantiate(rifleGrenade, attackPoint.position, Quaternion.identity);
        grenadeInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 10f; // 투사체 속도 설정

        grenadeInstance.GetComponent<Grenade>().Initialize(10f, 5f, 3f); // 데미지, 범위, 도트 지속 시간 초기화

        lastEActionTime = Time.time;
        StartCoroutine(CoolTimeE());
    }

    private IEnumerator CoolTimeE()
    {
        lastEActionTime = Time.time;

        while (Time.time - lastEActionTime < PlayerData.SkillECooldown)
        {
            Debug.Log($"E스킬 남은 시간 : {lastEActionTime}"); // 쿨타임 텍스트 갱신
            yield return null;
        }
        Debug.Log($"E스킬 쿨타임 완료"); // 쿨타임 완료 텍스트 갱신
    }

    //================================
}