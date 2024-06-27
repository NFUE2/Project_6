using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SwordE :MonoBehaviour, P_ISkill
{
    //투사체
    public GameObject projectile;

    public void SkillAction()
    {
        //투사체 복사 및 날리기
        
        Transform go = Instantiate(projectile).transform;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        go.LookAt(mousePos);
    }
}
