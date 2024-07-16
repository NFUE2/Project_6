using Photon.Pun;
using UnityEngine;
using System.Collections;

//클래스 이름이랑 파일이름이 다름, 변경필요
public class MacePlayer : PlayerBase
{
    [Header("Animation Data")]
    public Animator animator; // 향후 애니메이션 에셋 추가 => Sword를 위한 애니메이션 컨트롤러

    //스킬클래스로 이동 - 만약 스킬클래스에서 처리 못하면 말해주세요
    [Header("Skill Q")]
    private bool isGuard;
    private float lastQActionTime;
    public float qSkillCooldown;
    public float healDuration = 5f; // 힐 지속 시간
    public float healAmount = 10f; // 힐량
    public float statBoostDuration = 10f; // 스탯 강화 지속 시간
    public float defenseBoost = 10f; // 방어력 증가량

    [Header("Skill E")]
    private bool isDashing;
    private float lastEActionTime;
    public float eSkillCooldown;
    public float dashSpeed = 10f; // 돌진 속도
    public float dashDuration = 0.5f; // 돌진 지속 시간
    public float dashDamage = 20f; // 돌진 피해량
    public float reducedDamage = 0.5f; // 돌진 중 받는 피해 감소 비율
    public LayerMask bossLayer; // 보스 레이어
    private bool bossHit; // 보스 히트 여부
    //====================================

    [Header("Attack")]
    //공격부분 - 상위클래스로 이동
    public float attackTime;
    private float lastAttackTime;
    //====================================

    private bool enhancedAttack;

    //PlayerDataSO에서 가져오기
    public float health = 100f; // 기본 체력 값
    public float defense = 10f; // 기본 방어력 값
    //=========================================


    //근접 캐릭터 클래스에서 구현
    public override void Attack()
    {
        if (isGuard) return; // 가드 상태에서는 공격 불가
        if (Time.time - lastAttackTime < attackTime) return; // 공격 딜레이 체크
        Debug.Log("일반공격!");
        lastAttackTime = Time.time;

        float damage = 10f; // 기본 공격 데미지

        if (enhancedAttack)
        {
            damage += defense; // 방어력 추가 피해
            enhancedAttack = false; // 평타 강화 해제
            Debug.Log($"강화된 공격! 추가 피해: {defense}");
        }

        // 공격 애니메이션 트리거
        // animator.SetTrigger("Attack");

        // 데미지 처리 로직 추가 필요
    }

    //스킬클래스에서 구현
    public override void UseSkillQ()
    {
        if (Time.time - lastQActionTime < qSkillCooldown) return; // 쿨타임 체크
        StartCoroutine(HealAndBoost());
    }

    private IEnumerator HealAndBoost()
    {
        lastQActionTime = Time.time;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // 힐 범위

        int healedPlayers = 0;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                MacePlayer player = hitCollider.GetComponent<MacePlayer>();
                if (player != null && player != this)
                {
                    StartCoroutine(HealPlayer(player));
                    healedPlayers++;
                }
            }
        }

        if (healedPlayers > 0)
        {
            StartCoroutine(BoostDefense(healedPlayers));
        }

        yield return new WaitForSeconds(qSkillCooldown);
        Debug.Log($"Q스킬 쿨타임 완료"); // 쿨타임 완료 텍스트 갱신
    }

    private IEnumerator HealPlayer(MacePlayer player)
    {
        float startTime = Time.time;
        while (Time.time - startTime < healDuration)
        {
            player.health += healAmount * Time.deltaTime;
            player.health = Mathf.Clamp(player.health, 0, 100); // 체력 범위 제한
            Debug.Log($"힐량: {healAmount * Time.deltaTime}, 현재 체력: {player.health}");
            yield return null;
        }
    }

    private IEnumerator BoostDefense(int healedPlayers)
    {
        float originalDefense = defense; // 현재 방어력 저장
        defense += defenseBoost * healedPlayers;
        yield return new WaitForSeconds(statBoostDuration);
        defense = originalDefense; // 방어력 원상복구
    }

    public override void UseSkillE()
    {
        if (Time.time - lastEActionTime < eSkillCooldown || isDashing) return; // 쿨타임 체크 및 중복 실행 방지
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        lastEActionTime = Time.time;
        isDashing = true;
        bossHit = false;
        float startTime = Time.time;

        while (Time.time - startTime < dashDuration)
        {
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // 충돌 범위
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    //hitCollider.GetComponent<Enemy>().TakeDamage(dashDamage);
                    if (bossLayer == (bossLayer | (1 << hitCollider.gameObject.layer)))
                    {
                        bossHit = true;
                    }
                    else
                    {
                        // 적 밀어내기 및 스턴
                        Rigidbody enemyRb = hitCollider.GetComponent<Rigidbody>();
                        if (enemyRb != null)
                        {
                            Vector3 forceDirection = hitCollider.transform.position - transform.position;
                            forceDirection.y = 0;
                            enemyRb.AddForce(forceDirection.normalized * 5f, ForceMode.Impulse);
                        }
                    }
                }
            }

            yield return null;
        }

        isDashing = false;
        if (bossHit)
        {
            enhancedAttack = true; // 다음 공격 강화
        }

        yield return new WaitForSeconds(eSkillCooldown);
        Debug.Log($"E스킬 쿨타임 완료"); // 쿨타임 완료 텍스트 갱신
    }

    //==================================================
}
