using UnityEngine;

[CreateAssetMenu(fileName = "CraftItemDataSO", menuName = "Scriptable Object/CraftItemDataSO")]

public class CraftItemDataSO : ObjectSO
{

    public RequireItem[] requireItems;

    public int craftCost;

    public GameObject craftItem;

}
