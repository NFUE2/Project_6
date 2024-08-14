using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerBase : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    [Header("Skill Cooldown UI")]
    //public TextMeshProUGUI qCooldownText; // Q 스킬 쿨타임을 표시하는 UI 텍스트 요소
    //public TextMeshProUGUI eCooldownText; // E 스킬 쿨타임을 표시하는 UI 텍스트 요소
    public Image qCooldownImage; //Q 스킬 쿨타임을 표시하는 UI 이미지 요소
    public Image eCooldownImage; // E 스킬 쿨타임을 표시하는 UI 이미지 요소

    [Header("Animation Data")]
    protected Animator animator; // 향후 애니메이션 에셋 추가 =>

    [Header("Player Data")]
    public PlayerDataSO playerData; //플레이어 정보 받아오기

    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();

    public Image AttackcooldownBar;
    protected float currentAttackTime;

    private void Awake()
    {
        currentAttackTime = playerData.attackTime; // 초기 상태로 설정
        AttackcooldownBar.fillAmount = 1f; // 쿨타임 바를 초기 상태로 채움
    }
    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine) GameManager.instance.player = gameObject;

        GameManager.instance.players.Add(gameObject);
    }
    protected void AttackCoolTime()
    {
        // 쿨타임 진행 상태를 업데이트 바를 통해 업데이트
        if (currentAttackTime < playerData.attackTime)
        {
            currentAttackTime += Time.deltaTime;
            AttackcooldownBar.fillAmount = currentAttackTime / playerData.attackTime;
        }
    }

}
