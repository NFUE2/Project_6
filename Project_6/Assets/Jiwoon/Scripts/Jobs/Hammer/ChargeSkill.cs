using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

public class ChargeSkill : SkillBase
{
    public float maxChargingTime;
    public bool isCharging;
    public float damage;
    public float damageRate;
    public PlayerDataSO PlayerData;

    private bool isSkillAttack;
    private Animator animator;
    private float currentDamage;

    void Start()
    {
        animator = GetComponent<Animator>();
        cooldownDuration = PlayerData.SkillECooldown;
    }

    public override void UseSkill()
    {
        if (isCharging || Time.time - lastActionTime < cooldownDuration) return;

        isCharging = true;
        animator.SetBool("Charging", true);
        StartCoroutine(Charging());
    }

    private IEnumerator Charging()
    {
        float startCharging = Time.time;
        currentDamage = damage;
        while (!Input.GetKeyUp(KeyCode.E) && !(Time.time - startCharging > maxChargingTime))
        {
            currentDamage += Time.deltaTime * damageRate;
            yield return null;
        }
        isCharging = false;
        animator.SetBool("Charging", false);
        isSkillAttack = true;
        lastActionTime = Time.time;
        Smash(currentDamage);
    }

    private void Smash(float currentDamage)
    {
        this.currentDamage = currentDamage;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        P_Tentaclypse boss = collider.GetComponent<P_Tentaclypse>();
        if (boss != null)
        {
            if (isSkillAttack)
            {
                boss.TakeDamage(currentDamage);
                isSkillAttack = false;
            }
            else
            {
                boss.TakeDamage(damage);
            }
        }
        currentDamage = damage;
    }
}
