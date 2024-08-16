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
    public AudioClip attackSound;  // 공격 효과음 클립
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
            AttackcooldownBar.gameObject.SetActive(false); // 체력바 비활성화
        }
        else
        {
            AttackcooldownBar.gameObject.SetActive(true); // 체력바 활성화
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