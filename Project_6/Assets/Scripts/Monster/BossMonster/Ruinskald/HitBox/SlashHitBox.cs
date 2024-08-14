using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitBox : HitBox
{
    private void OnEnable()
    {
        duration = 0.3f;
        curDuration = 0;
    }
}
