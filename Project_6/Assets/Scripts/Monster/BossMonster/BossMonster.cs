using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class BossMonster : MonoBehaviourPun, IDamagable,IPunInstantiateMagicCallback
{
    public float maxHp { get; set; }
    public float attackPower { get; set; }
    public float defensePower { get; set; }
    public float moveSpeed {  get; set; }

    public Image hpBar;
    public float currentHp;

    public float GetFillAmountHP()
    {
        return (currentHp / maxHp);
    }

    public void TakeDamage(float damage)
    {
        currentHp -= (damage * (defensePower / 100));
        
        if (currentHp <= 0)
        {
            currentHp = 0;
            hpBar.fillAmount = 0;
            // »ç¸Á Ã³¸®
            //BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.DieState);

            //photonView.RPC(nameof(BossDie),RpcTarget.All);
            BossDie();
        }
        else
        {
            hpBar.fillAmount = GetFillAmountHP();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            BossBattleManager.instance.boss = this;
            BossBattleManager.instance.spawnedBoss = gameObject;
        }
    }

    //[PunRPC]
    protected void BossDie()
    {
        GameManager.instance.StageClear();
        BossBattleManager.instance.DestroyBoss();
    }
}