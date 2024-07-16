using Photon.Pun;
using UnityEngine;
using System.Collections;

public class HammerPlayer : PlayerBase
{
    //공격부분 - 상위클래스로 이동
    [Header("Attack")]
    public float attackTime;
    private float lastAttackTime;
    //====================================

    [Header("Animation Data")]
    public Animator animator; // 향후 애니메이션 에셋 추가 => Sword를 위한 애니메이션 컨트롤러

    //스킬클래스로 이동 - 만약 스킬클래스에서 처리 못하면 말해주세요
    [Header("Skill Q")]
    public GameObject shield;
    private GameObject createShield;
    //public float shieldTime;

    [Header("Skill E")]
    public float maxChargingTime;
    public bool isCharging;
    private bool isSkillAttack; // 스킬 공격 여부를 나타내는 플래그
    public float damage;
    public float damageRate;
    //====================================

    //근접 캐릭터 클래스에서 구현
    public override void Attack()
    {
        if (isCharging) return;
        if (Time.time - lastAttackTime < attackTime) return;

        lastAttackTime = Time.time;
        // 공격 애니메이션 재생
        animator.SetTrigger("Attack");
    }


    //스킬 클래스에서 구현
    public override void UseSkillQ()
    {
        if (createShield != null) return;
        //if (Time.time - lastQActionTime < qSkillCooldown) return;
        //createShield = Instantiate(shield, transform.position, Quaternion.identity);
        createShield = PhotonNetwork.Instantiate("Prototype/" + shield.name, transform.position, Quaternion.identity);
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        createShield.transform.localEulerAngles = new Vector3(0, 0, angle);

        //Invoke("ShieldDestroy", shieldTime);
        StartCoroutine(CoolTimeQ());
    }
    private void Update()
    {
        if (createShield != null && createShield.activeInHierarchy) createShield.transform.position = transform.position;
    }

    //private void ShieldDestroy()
    //{
    //    if (createShield != null) Destroy(createShield);
    //}
    IEnumerator CoolTimeQ()
    {
        lastQActionTime = Time.time;

        //while (Time.time - lastQActionTime < qSkillCooldown)
        {
            Debug.Log($"Q스킬 남은 시간 : {lastQActionTime}"); // 쿨타임 텍스트 갱신
            yield return null;
        }
        Debug.Log($"Q스킬 쿨타임 완료");// 쿨타임 완료 텍스트 갱신
    }

    public override void UseSkillE()
    {
        if (isCharging) return;
        //if (Time.time - lastEActionTime < eSkillCooldown) return;

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
        StartCoroutine(CoolTimeE());
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

    IEnumerator CoolTimeE()
    {
        lastEActionTime = Time.time;

        //while (Time.time - lastEActionTime < eSkillCooldown)
        {
            Debug.Log($"E스킬 남은 시간 : {lastEActionTime}"); // 쿨타임 텍스트 갱신
            yield return null;
        }
        Debug.Log($"E스킬 쿨타임 완료"); // 쿨타임 완료 텍스트 갱신
    }
    //=========================================
}
