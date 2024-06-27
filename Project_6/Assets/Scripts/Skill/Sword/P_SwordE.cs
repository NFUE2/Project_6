using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SwordE :MonoBehaviour, P_ISkill
{
    //����ü
    public GameObject projectile;

    public void SkillAction()
    {
        //����ü ���� �� ������
        
        Transform go = Instantiate(projectile).transform;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        go.LookAt(mousePos);
    }
}
