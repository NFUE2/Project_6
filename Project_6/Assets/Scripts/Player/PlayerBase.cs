using Photon.Pun;
using TMPro;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public PlayerDataSO playerData;

    public TextMeshProUGUI qCooldownText; // Q ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���
    public TextMeshProUGUI eCooldownText; // E ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���
    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();

    [Header("Animation Data")]
    public Animator animator; // ���� �ִϸ��̼� ���� �߰� =>

    [Header("Skills")]
    protected float lastQActionTime;  // Q ��ų ������ ��� �ð�
    protected float lastEActionTime;  // E ��ų ������ ��� �ð�

    [Header("Attack")]
    protected PlayerDataSO attackTime; // ���� �ð� ���� -> PlayerDataSO���� �޾Ƽ� ���
    protected float lastAttackTime;  // ������ ���� �ð�

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
