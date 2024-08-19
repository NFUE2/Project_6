using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class RiflePlayer : RangedPlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private TargetSkill targetSkill; // TargetSkill 인스턴스

    [Header("Skill E")]
    [SerializeField] private GrenadeSkill grenadeSkill; // GrenadeSkill 인스턴스

    private bool isTargeting; // 타겟팅 모드 플래그
    private float targetingStartTime; // 타겟팅 모드 시작 시간
    private float targetingDuration = 5f; // 타겟팅 모드 제한 시간 (초)

    private void Start()
    {
        //targetSkill.SetCooldownText(qCooldownText);
        //grenadeSkill.SetCooldownText(eCooldownText);

        targetSkill.SetCooldownImage(qCooldownImage);
        grenadeSkill.SetCooldownImage(eCooldownImage);

        // TargetSkill에 RiflePlayer 참조를 전달하여 타겟팅 모드 설정
        targetSkill.Initialize(this);
    }

    private void Update()
    {
        AttackCoolTime();
        // 타겟팅 모드에서 마우스 클릭을 감지하여 타겟이 없는 곳을 클릭하면 타겟팅 모드 종료
        if (isTargeting && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Collider2D hit = Physics2D.OverlapPoint(mousePosition, targetSkill.enemyLayerMask);
            if (hit == null)
            {
                SetTargeting(false);
            }
        }

        // 타겟팅 모드에서 제한 시간이 지나면 타겟팅 모드 종료
        if (isTargeting && Time.time - targetingStartTime > targetingDuration)
        {
            SetTargeting(false);
        }
    }

    public override void UseSkillQ()
    {
        isTargeting = true; // 타겟팅 모드 활성화
        targetingStartTime = Time.time; // 타겟팅 모드 시작 시간 설정
        targetSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        grenadeSkill.UseSkill();
    }

    public override void Attack()
    {
        if (isTargeting) return; // 타겟팅 모드일 때 공격 무시
        base.Attack(); // 기본 공격 로직 호출
    }


    public void SetTargeting(bool targeting)
    {
        isTargeting = targeting;

        // 타겟팅 모드가 비활성화되면 모든 타겟 마커를 파괴
        if (!targeting)
        {
            targetSkill.ClearTargetMarkers();
        }
    }
}
