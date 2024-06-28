using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SwordQ : MonoBehaviour, P_ISkill
{
    private Animator animator; //ĳ���� �ִϸ��̼� ����

    public bool isGuard { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SkillAction()
    {
        if(isGuard) ExitGuard();
        else
        {
            Debug.Log("Q��ų���");
            //animator.SetBool("SkillQ", isGuard = true);
        }
    }

    public void ExitGuard()
    {
        isGuard = false;
        //animator.SetBool("SkillQ", isGuard = false);
    }
}
