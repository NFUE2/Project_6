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
    public bool isCharging;

    Coroutine charging;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SkillAction()
    {
        if (isCharging) return;

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

        //animator.SetBool("Charging", isCharging = false);
        //Smash();
        //������ �ִ� �κ��� ��ũ��Ʈ�� ���� �ִϸ��̼� �̺�Ʈ�� ó������ ���
    }

    public void Smash()
    {

        //������ �Էºκ� �ʿ�
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 dir = (sr.flipX ? Vector2.right : Vector2.left); 

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir,attackDistance);

        charging = null;
    }
}
