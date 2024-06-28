using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_GunE : MonoBehaviour, P_ISkill
{
    Animator animator;
    Rigidbody2D rigidbody;
    public float rollingX;

    public bool isRolling { get; private set; } 

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SkillAction()
    {
        animator.SetTrigger("SkillE");
        isRolling = true;
        rigidbody.velocity = new Vector2(rollingX,0);
        //SkillEAction?.Invoke();
    }

    public void ExitRolling()
    {
        isRolling = false;
    }
}
