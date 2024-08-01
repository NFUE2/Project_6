using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class BossMonster : MonoBehaviour, IDamagable,IPunInstantiateMagicCallback
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
            // 사망 처리
            BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.DieState);
        }
        else
        {
            hpBar.fillAmount = GetFillAmountHP();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("작동");
        if(!PhotonNetwork.IsMasterClient)
        {
            BossBattleManager.instance.boss = this;
            BossBattleManager.instance.spawnedBoss = gameObject;
        }
    }
}