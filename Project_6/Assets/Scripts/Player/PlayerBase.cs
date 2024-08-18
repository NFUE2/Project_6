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
    public AudioClip attackSound;  // 공격 효과음 클립
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

            // 쿨타임 바 활성화
            AttackcooldownBar.gameObject.SetActive(true);
        }
        else
        {
            // 쿨타임이 다 찼을 때 비활성화
            AttackcooldownBar.gameObject.SetActive(false);
            currentAttackTime = playerData.attackTime; // 이 부분은 안정성을 위해 추가
        }
    }

    protected void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            SoundManager.Instance.Shot(clip); // SoundManager를 통해 SFX 재생
        }
    }
}