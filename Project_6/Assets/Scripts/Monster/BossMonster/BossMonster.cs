using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class BossMonster : MonoBehaviourPun, IDamagable,IPunDamagable,IPunInstantiateMagicCallback
{
    public float maxHp { get; set; }
    public float attackPower { get; set; }
    public float defensePower { get; set; }
    public float moveSpeed {  get; set; }

    public Image hpBar;
    public float currentHp;

    public float SetMultiHP()
    {
        if(GameManager.Instance.players.Count != 0)
        {
            if(GameManager.Instance.players.Count == 1)
            {
                return 1;
            }
            else
            {
                return (float)(2 + (0.25 * (GameManager.Instance.players.Count - 2)));
            }
        }
        else
        {
            return 1;
        }
    }

    public float GetFillAmountHP()
    {
        return (currentHp / maxHp);
    }

    public void TakeDamage(float damage)
    {
        currentHp -= (damage - (damage * (defensePower / 100)));
        
        if (currentHp <= 0)
        {
            currentHp = 0;
            hpBar.fillAmount = 0;
            // »ç¸Á Ã³¸®
            //BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.DieState);

            if(photonView.IsMine) photonView.RPC(nameof(BossDie), RpcTarget.All);
            //BossDie();
        }
        else
        {
            if(hpBar != null)
            {
                hpBar.fillAmount = GetFillAmountHP();
            }
            
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            BossBattleManager.instance.boss = this;
            BossBattleManager.instance.spawnedBoss = gameObject;
            BossBattleManager.instance.attackController = GetComponent<BossAttackController>();
            BossBattleManager.instance.bossAnimator = GetComponent<Animator>();
            BossBattleManager.instance.bossStateMachine = GetComponent<BossStateMachine>();
        }
    }

    [PunRPC]
    protected void BossDie()
    {
        GameManager.instance.StageClear();
        BossBattleManager.instance.DestroyBoss();
    }

    public void Damage(float damage)
    {
        photonView.RPC(nameof(DamageRPC), RpcTarget.All, damage);
    }

    [PunRPC]
    public void DamageRPC(float damage)
    {
        TakeDamage(damage);
    }
}