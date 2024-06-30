using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_HammerE : MonoBehaviour, P_ISkill
{
    private float damage;
    private float damageRate;
    public  float maxChargingTime = 1f;
    private float attackDistance;

    private Animator animator;
    public bool isCharging;

    Coroutine charging;

    public float actionTime;
    private float lastAction;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        lastAction = -actionTime;
    }

    public void SkillAction()
    {
        if (isCharging) return;
        if (Time.time - lastAction < actionTime) return;

        animator.SetBool("Charging", isCharging = true);
        StartCoroutine(Charging());
    }

    IEnumerator Charging()
    {
        //animator.SetBool("Charging", isCharging);
        float startCharging = Time.time;

        while (!Input.GetKeyUp(KeyCode.E) && !(Time.time - startCharging > maxChargingTime))
        {
            damage += Time.deltaTime * damageRate;
            yield return null;
        }
        animator.SetBool("Charging", isCharging = false);
        StartCoroutine(CoolTime());

        //animator.SetBool("Charging", isCharging = false);
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
    IEnumerator CoolTime()
    {
        lastAction = Time.time;
        Text coolTimeText = GetComponent<PlayerController_Hammer>().cooltimeEText;

        while (Time.time - lastAction < actionTime)
        {
            coolTimeText.text = (actionTime - (Time.time - lastAction)).ToString("F1");
            yield return null;
        }

        coolTimeText.text = "준비완료";
    }

}
