using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_GunQ : MonoBehaviour, P_ISkill
{
    //플레이어 객체 가져와야함

    public bool fanningReady { get; private set; }

    public void SkillAction()
    {
        //재장전 스크립트 필요
        fanningReady = true;
    }
}
