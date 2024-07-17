using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MonsterLongAttack : MonsterAttack
{
    //public MonsterLongAttack(MonsterStateMachine stateMachine) : base(stateMachine) { }
    public Transform fire;

    public override void Attack()
    {
        Vector2 direction = Direction(controller.target.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //string projectileName = data.projectile.name; //����ü �̸�
        //GameObject proejectile = PhotonNetwork.Instantiate($"Projectile/{projectileName}", fire.position, Quaternion.identity); //������ ����ü
        GameObject proejectile = Instantiate(data.projectile, fire.position, Quaternion.identity); //������ ����ü
        proejectile.transform.localEulerAngles = new Vector3(0, 0, -angle); //����ü ����
    }
}