using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;

public class RiflePlayer : RangedPlayerBase
{
    public PlayerData PlayerData; //�ֻ��� ���� ó��


    //�ش� Ŭ������ ���� Ŭ������ �޾ƿ��ּ���
    [Header("Skill Q")]
    [SerializeField] private TargetSkill targetSkill; // TargetSkill �ν��Ͻ�

    [Header("Skill E")]
    [SerializeField] private GrenadeSkill grenadeSkill; // GrenadeSkill �ν��Ͻ�


    private void Start()
    {
        mainCamera = Camera.main; //���� RangedPlayerBase���� ó����
        targetSkill.SetPlayer(this);
        grenadeSkill.SetPlayer(this);
    }

    // ��Ʈ �׼� �����̾ attackCount�� ����ϴ°� ������ �׷������ð� �Ϲ� ���Ÿ� ���ݰ� �����ϰ� ó���ϸ�˴ϴ�.
    public override void Attack()
    {
        //if (isAttackCooldown) return; //����

        //if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time; 

        //attackCount++; //����

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //�Է¿��� ó���Ѱ��� �������� ������� ó��
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity);
        attackInstance.GetComponent<Projectile>().SetDirection(attackDirection); //ȸ��ó���� ����

        //����
        //if (attackCount >= 1) 
        {
            //isAttackCooldown = true;
            lastAttackTime = Time.time;
        }
        //=============================
    }
    

    //����
    private void Update()
    {
        //UpdateCooldown();
    }
    //=========================

    public override void UseSkillQ()
    {
        targetSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        grenadeSkill.UseSkill();
    }
}
