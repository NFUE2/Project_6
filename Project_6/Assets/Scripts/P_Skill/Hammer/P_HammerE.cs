using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_HammerE : MonoBehaviour, P_ISkill
{
    private float damage;
    private float damageRate;
    public  float maxChargingTime = 1f;
    private float attackDistance;

    private Animator animator;
    private bool isCharging;

    Coroutine charging;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SkillAction()
    {
        isCharging = true;

        if(charging == null)
            StartCoroutine(Charging());
    }

    IEnumerator Charging()
    {
        animator.SetBool("Charging", isCharging);
        float startCharging = Time.time;

        while (!Input.GetKeyUp(KeyCode.E) && !(Time.time - startCharging > maxChargingTime))
        {
            damage += Time.deltaTime * damageRate;
            yield return null;
        }

        animator.SetBool("Charging", isCharging = false);
        //Smash();
        //데미지 주는 부분을 스크립트로 할지 애니메이션 이벤트로 처리할지 고민
    }

    public void Smash()
    {

        //데미지 입력부분 필요
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 dir = (sr.flipX ? Vector2.right : Vector2.left); 

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir,attackDistance);

        charging = null;
    }
}
