using UnityEngine;
using UnityEngine.UI;

public class BossMonster : MonoBehaviour, IDamagable
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
            BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.DieState);
        }
        else
        {
            hpBar.fillAmount = GetFillAmountHP();
        }
    }
}