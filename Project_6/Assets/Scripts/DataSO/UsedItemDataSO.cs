using UnityEngine;

[CreateAssetMenu(fileName = "UsedItemDataSO", menuName = "Scriptable Object/UsedItemDataSO")]
public class UsedItemDataSO : ItemDataSO
{
    //public UsedType usedType;
    public StateType stateType;

    public ApplyType applyType;

    public float value;
}
