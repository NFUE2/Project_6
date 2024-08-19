using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    protected PlayerInput playerInput;

    protected virtual void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        currentAttackTime = playerData.attackTime;
        AttackcooldownBar.fillAmount = 1f;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        AttackCoolTime();
    }

    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine) GameManager.instance.player = gameObject;
        GameManager.instance.players.Add(gameObject);
    }

    protected virtual void AttackCoolTime()
    {
        if (currentAttackTime < playerData.attackTime)
        {
            currentAttackTime += Time.deltaTime;
            AttackcooldownBar.fillAmount = currentAttackTime / playerData.attackTime;

            // ��Ÿ�� �� Ȱ��ȭ
            AttackcooldownBar.gameObject.SetActive(true);
        }
        else
        {
            // ��Ÿ���� �� á�� �� ��Ȱ��ȭ
            AttackcooldownBar.gameObject.SetActive(false);
            currentAttackTime = playerData.attackTime; // �� �κ��� �������� ���� �߰�
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