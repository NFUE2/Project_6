using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MonsterLongAttack : MonsterAttack
{
    public MonsterLongAttack(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Attack()
    {
        Vector2 myPos = stateMachine.controller.transform.position;
        Vector2 targetPos = stateMachine.controller.target.position;
        Vector2 direction = (targetPos - myPos).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        string projectileName = stateMachine.controller.data.projectile.name; //����ü �̸�
        GameObject proejectile = PhotonNetwork.Instantiate($"Projectile/{projectileName}", myPos, Quaternion.identity); //������ ����ü
        proejectile.transform.localEulerAngles = new Vector3(0, 0, angle); //����ü ����
    }
}