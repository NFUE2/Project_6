using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerBase : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    [Header("Skill Cooldown UI")]
    //public TextMeshProUGUI qCooldownText; // Q ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���
    //public TextMeshProUGUI eCooldownText; // E ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���
    public Image qCooldownImage; //Q ��ų ��Ÿ���� ǥ���ϴ� UI �̹��� ���
    public Image eCooldownImage; // E ��ų ��Ÿ���� ǥ���ϴ� UI �̹��� ���

    [Header("Animation Data")]
    protected Animator animator; // ���� �ִϸ��̼� ���� �߰� =>

    [Header("Player Data")]
    public PlayerDataSO playerData; //�÷��̾� ���� �޾ƿ���

    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();

    public Image AttackcooldownBar;
    protected float currentAttackTime;

    private void Awake()
    {
        currentAttackTime = playerData.attackTime; // �ʱ� ���·� ����
        AttackcooldownBar.fillAmount = 1f; // ��Ÿ�� �ٸ� �ʱ� ���·� ä��
    }
    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine) GameManager.instance.player = gameObject;

        GameManager.instance.players.Add(gameObject);
    }
    protected void AttackCoolTime()
    {
        // ��Ÿ�� ���� ���¸� ������Ʈ �ٸ� ���� ������Ʈ
        if (currentAttackTime < playerData.attackTime)
        {
            currentAttackTime += Time.deltaTime;
            AttackcooldownBar.fillAmount = currentAttackTime / playerData.attackTime;
        }
    }

}
