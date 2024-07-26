

public class EnhanceItem : ItemBase<UsedItemDataSO>, IUsableItem
{
    public void UseItem()
    {
        PlayerCondition con = GameManager.instance.player.GetComponent<PlayerCondition>();
        PlayerDataSO pData = GameManager.instance.player.GetComponent<PlayerBase>().playerData;

        switch (data.stateType)
        {
            case StateType.MaxHP:
                break;
            case StateType.MoveSpeed:
                break;
            case StateType.AttackTime:
                break;
            case StateType.AttackDamage:
                break;
            case StateType.Skill_Q_CoolTimeDecrease:
                break;
            case StateType.Skill_E_CoolTimeDecrease:
                break;
        }
    }
}
