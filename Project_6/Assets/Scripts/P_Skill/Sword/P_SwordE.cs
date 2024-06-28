using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class P_SwordE :MonoBehaviour, P_ISkill
{
    //투사체
    public GameObject projectile;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SkillAction()
    {
        //투사체 복사 및 날리기
        animator.SetTrigger("SkillE");

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;

        Transform go = Instantiate(projectile).transform;

        go.localEulerAngles = new Vector3(0,0,angle);
    }
}
