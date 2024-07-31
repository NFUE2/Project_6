using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : ItemBase<UsedItemDataSO>, IUsableItem
{
    public void UseItem()
    {
        IHealable p = GameManager.instance.player.GetComponent<IHealable>();

        if (data.applyType == ApplyType.Amount)
        {
            p.TakeHeal(data.value);
        }
        else
        {
            PlayerCondition con = GameManager.instance.player.GetComponent<PlayerCondition>();
            float value = con.maxHealth * (data.value / 100);

            p.TakeHeal(value);
        }
    }
}
