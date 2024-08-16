using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerBase : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    [Header("Skill Cooldown UI")]
    public Image qCooldownImage;
    public Image eCooldownImage;

    [Header("Player Data")]
    public PlayerDataSO playerData;

    [Header("Player Sound")]
    public AudioClip attackSound;  // ���� ȿ���� Ŭ��
    protected AudioSource audioSource;

    public abstract void Attack();
    public abstract void UseSkillQ();
    public abstract void UseSkillE();

    public Image AttackcooldownBar;
    protected float currentAttackTime;

    protected virtual void Awake()
    {
        currentAttackTime = playerData.attackTime;
        AttackcooldownBar.fillAmount = 1f;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        AttackCoolTime();
        HandleHealthBarVisibility();
    }

    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine) GameManager.instance.player = gameObject;
        GameManager.instance.players.Add(gameObject);
    }

    protected void AttackCoolTime()
    {
        if (currentAttackTime < playerData.attackTime)
        {
            currentAttackTime += Time.deltaTime;
            AttackcooldownBar.fillAmount = currentAttackTime / playerData.attackTime;
        }
    }

    private void HandleHealthBarVisibility()
    {
        if (AttackcooldownBar.fillAmount >= 1f)
        {
            AttackcooldownBar.gameObject.SetActive(false); // ü�¹� ��Ȱ��ȭ
        }
        else
        {
            AttackcooldownBar.gameObject.SetActive(true); // ü�¹� Ȱ��ȭ
        }
    }

    protected void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            SoundManager.Instance.Shot(clip); // SoundManager�� ���� SFX ���
        }
    }
}