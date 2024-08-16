using UnityEngine;

public class TestBossDamageButton : MonoBehaviour
{
    public void DamageToBoss()
    {
        float damage = 100;
        BossTestManager.Instance.boss.TakeDamage(damage);
        Debug.Log(BossTestManager.Instance.boss.currentHp);
    }
}