using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_GunQ : MonoBehaviour, P_ISkill
{
    //�÷��̾� ��ü �����;���

    public bool fanningReady { get; private set; }

    public void SkillAction()
    {
        //������ ��ũ��Ʈ �ʿ�
        fanningReady = true;
    }
}
