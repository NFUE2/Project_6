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
        AttackClip();
        if (!PhotonNetwork.IsMasterClient) return;

        Vector2 direction = Direction(fire.position,controller.target.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject proejectile = PhotonNetwork.Instantiate(data.projectile.name, fire.position, Quaternion.Euler(0, 0, angle)); //복사할 투사체
    }
}