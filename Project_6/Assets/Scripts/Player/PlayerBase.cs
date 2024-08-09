using Photon.Pun;
using TMPro;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public PlayerDataSO playerData;

    public TextMeshProUGUI qCooldownText; // Q 스킬 쿨타임을 표시하는 UI 텍스트 요소
    public TextMeshProUGUI eCooldownText; // E 스킬 쿨타임을 표시하는 UI 텍스트 요소
    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();

    [Header("Animation Data")]
    public Animator animator; // 향후 애니메이션 에셋 추가 =>

    [Header("Skills")]
    protected float lastQActionTime;  // Q 스킬 마지막 사용 시간
    protected float lastEActionTime;  // E 스킬 마지막 사용 시간

    [Header("Attack")]
    protected PlayerDataSO attackTime; // 공격 시간 간격 -> PlayerDataSO에서 받아서 사용
    protected float lastAttackTime;  // 마지막 공격 시간

    public void SetLastEActionTime(float time)
    {
        lastEActionTime = time;
    }

    public float GetLastEActionTime()
    {
        return lastEActionTime;
    }

    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine) GameManager.instance.player = gameObject;

        GameManager.instance.players.Add(gameObject);
    }
}
