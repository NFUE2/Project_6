using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitBox : HitBox
{
    private void OnEnable()
    {
        originalSpeed = BossBattleManager.Instance.boss.moveSpeed;
        mv.speed = 0f;
        duration = 0.3f;
        curDuration = 0;
    }
}
