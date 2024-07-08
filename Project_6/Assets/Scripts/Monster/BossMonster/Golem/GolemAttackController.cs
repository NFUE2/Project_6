using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttackController : BossAttackController
{
    public override void SelectAttack()
    {
        base.SelectAttack();
        countOfAttack = 2;
        int index = Random.Range(0, countOfAttack);
        switch (index)
        {
            case 0:
                EyeRazor();
                break;
            case 1:
                Punch();
                break;
        }
    }
    public void EyeRazor()
    {
        Debug.Log("∑π¿Ã¿˙ ∫ˆ!");
    }

    public void Punch()
    {
        Debug.Log("»÷¿˚... »÷¿˚...");
    }
}
