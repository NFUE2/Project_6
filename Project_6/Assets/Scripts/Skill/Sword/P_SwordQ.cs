using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SwordQ : MonoBehaviour, P_ISkill
{
    private Animator animator; //ĳ���� �ִϸ��̼� ����

    public bool isGuard { get; private set; }
    public void SkillAction()
    {
        //ĳ���� ���¸� �����������
        isGuard = true;
        //animator.SetBool("SkillQ");
    }

    public void ExitGuard()
    {
        isGuard = false;
    }
}
