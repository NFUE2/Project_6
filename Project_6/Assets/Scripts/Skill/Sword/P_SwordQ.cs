using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SwordQ : MonoBehaviour, P_ISkill
{
    private Animator animator; //캐릭터 애니메이션 변경

    public bool isGuard { get; private set; }
    public void SkillAction()
    {
        //캐릭터 상태를 변경해줘야함
        isGuard = true;
        //animator.SetBool("SkillQ");
    }

    public void ExitGuard()
    {
        isGuard = false;
    }
}
