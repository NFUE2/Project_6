using UnityEngine;


[CreateAssetMenu(fileName = "UsedItemDataSO", menuName = "Scriptable Object/UsedItemDataSO")]
public class UsedItemDataSO : ItemDataSO
{

    public UsedType usedType;

    public ApplyType applyType;

    public float value;

}
