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
    private bool isSkillAttack; // 스킬 공격 여부를 나타내는 플래그

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
        isSkillAttack = true; // 스킬 공격 플래그 설정
        StartCoroutine(CoolTime());
        Smash(currentDamage);
    }

    public void Smash(float currentDamage)
    {
        // 스킬 공격 데미지를 저장
        this.currentDamage = currentDamage;
    }

    private float currentDamage;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log($"충돌된 객체: {collider.name}");
        P_Tentaclypse boss = collider.GetComponent<P_Tentaclypse>();
        if (boss != null)
        {
            if (isSkillAttack)
            {
                Debug.Log($"스킬 데미지 {currentDamage} 만큼 넣었습니다");
                boss.TakeDamage(currentDamage);
                isSkillAttack = false; // 스킬 공격 플래그 초기화
            }
            else
            {
                Debug.Log($"일반 공격 데미지 {damage} 만큼 넣었습니다");
                boss.TakeDamage(damage);
            }
        }
        else
        {
            //Debug.Log($"적 오브젝트가 아닙니다: {collider.name}");
        }
        currentDamage = damage; // 데미지 초기화
    }

    IEnumerator CoolTime()
    {
        lastAction = Time.time;
        //Text coolTimeText = GetComponent<PlayerController_Hammer>().cooltimeEText;

        while (Time.time - lastAction < actionTime)
        {
            //coolTimeText.text = (actionTime - (Time.time - lastAction)).ToString("F1");
            yield return null;
        }

        //coolTimeText.text = "준비완료";
    }
}
