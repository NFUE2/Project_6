using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;

public class RiflePlayer : RangedPlayerBase
{
    [Header("Skill Q")]
    [SerializeField] private TargetSkill targetSkill; // TargetSkill �ν��Ͻ�

    [Header("Skill E")]
    [SerializeField] private GrenadeSkill grenadeSkill; // GrenadeSkill �ν��Ͻ�

    private bool isTargeting; // Ÿ���� ��� �÷���
    private float targetingStartTime; // Ÿ���� ��� ���� �ð�
    private float targetingDuration = 5f; // Ÿ���� ��� ���� �ð� (��)

    private void Start()
    {
        targetSkill.SetCooldownText(qCooldownText);
        grenadeSkill.SetCooldownText(eCooldownText);

        // TargetSkill�� RiflePlayer ������ �����Ͽ� Ÿ���� ��� ����
        targetSkill.Initialize(this);
    }

    private void Update()
    {
        // Ÿ���� ��忡�� ���콺 Ŭ���� �����Ͽ� Ÿ���� ���� ���� Ŭ���ϸ� Ÿ���� ��� ����
        if (isTargeting && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Collider2D hit = Physics2D.OverlapPoint(mousePosition, targetSkill.enemyLayerMask);
            if (hit == null)
            {
                SetTargeting(false);
            }
        }

        // Ÿ���� ��忡�� ���� �ð��� ������ Ÿ���� ��� ����
        if (isTargeting && Time.time - targetingStartTime > targetingDuration)
        {
            SetTargeting(false);
        }
    }

    public override void UseSkillQ()
    {
        Debug.Log("UseSkillQ ȣ���"); // ����� �޽��� �߰�
        isTargeting = true; // Ÿ���� ��� Ȱ��ȭ
        targetingStartTime = Time.time; // Ÿ���� ��� ���� �ð� ����
        targetSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        grenadeSkill.UseSkill();
    }

    public override void Attack()
    {
        if (isTargeting) return; // Ÿ���� ����� �� ���� ����
        base.Attack();
    }

    public void SetTargeting(bool targeting)
    {
        isTargeting = targeting;

        // Ÿ���� ��尡 ��Ȱ��ȭ�Ǹ� ��� Ÿ�� ��Ŀ�� �ı�
        if (!targeting)
        {
            targetSkill.ClearTargetMarkers();
        }
    }
}
