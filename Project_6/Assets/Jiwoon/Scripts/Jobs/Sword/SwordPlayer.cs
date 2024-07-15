using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using TMPro;

public class SwordPlayer : MeleePlayerBase
{
    public PlayerData PlayerData;
    public TextMeshProUGUI qCooldownText; // Q ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���
    public TextMeshProUGUI eCooldownText; // E ��ų ��Ÿ���� ǥ���ϴ� UI �ؽ�Ʈ ���

    [Header("Animation Data")]
    public Animator animator; // ���� �ִϸ��̼� ���� �߰� => Sword�� ���� �ִϸ��̼� ��Ʈ�ѷ�

    [Header("Skill Q")]
    [SerializeField] private GuardSkill guardSkill; // GuardSkill �ν��Ͻ�

    [Header("Skill E")]
    [SerializeField] private ProjectileSkill projectileSkill; // ProjectileSkill �ν��Ͻ�

    private void Start()
    {
        guardSkill.SetCooldownText(qCooldownText);
        projectileSkill.SetCooldownText(eCooldownText);
    }

    public override void UseSkillQ()
    {
        guardSkill.UseSkill();
    }

    public override void UseSkillE()
    {
        projectileSkill.UseSkill();
    }
}
