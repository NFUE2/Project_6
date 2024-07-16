using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine;

public class RiflePlayer : RangedPlayerBase
{
    public PlayerData PlayerData; //최상위 에서 처리


    //해당 클래스의 상위 클래스를 받아와주세요
    [Header("Skill Q")]
    [SerializeField] private TargetSkill targetSkill; // TargetSkill 인스턴스

    [Header("Skill E")]
    [SerializeField] private GrenadeSkill grenadeSkill; // GrenadeSkill 인스턴스


    private void Start()
    {
        mainCamera = Camera.main; //삭제 RangedPlayerBase에서 처리중
        targetSkill.SetPlayer(this);
        grenadeSkill.SetPlayer(this);
    }

    // 볼트 액션 소총이어서 attackCount를 사용하는것 같은데 그러지마시고 일반 원거리 공격과 동일하게 처리하면됩니다.
    public override void Attack()
    {
        //if (isAttackCooldown) return; //삭제

        //if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time; 

        //attackCount++; //삭제

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //입력에서 처리한값을 가져오는 방식으로 처리
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity);
        attackInstance.GetComponent<Projectile>().SetDirection(attackDirection); //회전처리로 변경

        //삭제
        //if (attackCount >= 1) 
        {
            //isAttackCooldown = true;
            lastAttackTime = Time.time;
        }
        //=============================
    }
    

    //삭제
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
