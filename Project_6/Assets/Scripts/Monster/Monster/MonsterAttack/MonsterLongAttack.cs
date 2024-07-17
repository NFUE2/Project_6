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
        Vector2 myPos = transform.position;
        Vector2 targetPos = controller.target.position;
        Vector2 direction = (targetPos - myPos).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        string projectileName = data.projectile.name; //투사체 이름

        GameObject proejectile = PhotonNetwork.Instantiate($"Projectile/{projectileName}", fire.position, Quaternion.identity); //복사할 투사체
        proejectile.transform.localEulerAngles = new Vector3(0, 0, angle); //투사체 각도
    }
}