using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class P_SwordQ : MonoBehaviour, P_ISkill
{
    private Animator animator; //ĳ���� �ִϸ��̼� ����
    public float actionTime;
    private float lastAction;

    public bool isGuard { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        lastAction = -actionTime;
    }

    public void SkillAction()
    {
        if(isGuard) ExitGuard();
        else
        {
            if (Time.time - lastAction < actionTime) return;

            Debug.Log("Q��ų���");
            //isGuard = true;
            animator.SetBool("Guard", isGuard = true);
            Invoke("ExitGuardEvent", 1.0f);
        }
    }

    public void ExitGuard()
    {
        Debug.Log("��ų����");
        animator.SetBool("Guard", isGuard = false);
        //isGuard = false;
        StartCoroutine(CoolTime());
    }

    IEnumerator CoolTime()
    {
        lastAction = Time.time;
        //Text coolTimeText = GetComponent<PlayerController_Melee>().cooltimeQText;

        while(Time.time  - lastAction < actionTime)
        {
            //coolTimeText.text = (actionTime - (Time.time - lastAction)).ToString("F1");
            yield return null;
        }

        //coolTimeText.text = "�غ�Ϸ�";
    }

    private void ExitGuardEvent()
    {
        if(isGuard) ExitGuard();
    }
}