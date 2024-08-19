using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordExplosionHitBox : HitBox
{
    
    private void OnEnable()
    {
        originalSpeed = BossBattleManager.Instance.boss.moveSpeed;
        mv.speed = 0f;
        curDuration = 0;
        duration = 0.4f;
    }
}
