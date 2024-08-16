using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : NPCBase
{
    public override void Interaction()
    {
        GameObject player = GameManager.instance.player;

        if(player.TryGetComponent(out PlayerCondition p))
            p.Heal(p.maxHealth);
        {
            if(p.input.isDead)p.Resurrection();
            else p.Heal(p.PlayerData.maxHP);
        }
    }
}
