
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Object/ItemDataSO")]
public class ItemDataSO : ObjectSO
{
    
    public int buyCost;
    public int sellCost;

    public bool stackable;
    public int maxStackSize;
    public Sprite itemImage;


}
