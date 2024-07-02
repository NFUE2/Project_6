using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_HammerE : MonoBehaviour, P_ISkill
{
    public float damage = 10f;
    public float damageRate;
    public float maxChargingTime;
    public float attackDistance;

    private Animator animator;
    public bool isCharging;

    Coroutine charging;

    public float actionTime;
    private float lastAction;

    private void Awake()
    {
        BoxCollider2D boxCollider = GetComponentInChildren<BoxCollider2D>();
     
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
        animator.SetBool("Charging", isCharging);
        float startCharging = Time.time;
        float currentDamage = damage;
        while (!Input.GetKeyUp(KeyCode.E) && !(Time.time - startCharging > maxChargingTime))
        {
            currentDamage += Time.deltaTime * damageRate;
            yield return null;
        }
        animator.SetBool("Charging", isCharging = false);
        StartCoroutine(CoolTime());
        Smash(currentDamage);

    }

    public void Smash(float currentDamage)
    {
        // 이 메서드에서 충돌을 판정하지 않고, OnTriggerEnter2D를 통해 충돌을 판정합니다.
        this.currentDamage = currentDamage; // 데미지를 저장
    }

    private float currentDamage;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log($"충돌된 객체: {collider.name}");
        P_Tentaclypse boss = collider.GetComponent<P_Tentaclypse>();
        if (boss != null)
        {
            Debug.Log($"스킬 데미지 {currentDamage} 만큼 넣었습니다");
            boss.TakeDamage(currentDamage);
        }
        else
        {
            //Debug.Log($"적 오브젝트가 아닙니다: {collider.name}");
        }
        currentDamage = damage;
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
