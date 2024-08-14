using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinskaldAttackController : BossAttackController
{
    public override void SelectAttack()
    {
        base.SelectAttack();
        int index = Random.Range(0, 3);

        switch (index)
        {
            case 0:
                Debug.Log("1번 패턴");
                break;
            case 1:
                Debug.Log("2번 패턴");
                break;
            case 2:
                Debug.Log("3번 패턴");
                break;
        }
    }

    // 견제형 패턴
    // 
}
