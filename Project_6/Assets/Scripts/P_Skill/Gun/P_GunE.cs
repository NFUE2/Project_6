using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class P_GunE : MonoBehaviour, P_ISkill
{
    //Animator animator;
    Rigidbody2D rigidbody;
    public float rollingX;

    //public bool isRolling { get; private set; } 

    private void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SkillAction()
    {
        //animator.SetTrigger("SkillE");
        //if (isRolling) return;
        if (GetComponent<PlayerController_Gun>().isRolling) return;
        //isRolling = true;
        GetComponent<PlayerController_Gun>().isRolling = true;

        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        int c = dir.x > 0 ? 1 : -1;  

        rigidbody.velocity = new Vector2(rollingX * c,0);

        Invoke("ExitRolling",1.0f);
        //SkillEAction?.Invoke();
    }

    public void ExitRolling()
    {
        GetComponent<PlayerController_Gun>().isRolling = false;

        //isRolling = false;
        rigidbody.velocity = Vector2.zero;
    }
}
