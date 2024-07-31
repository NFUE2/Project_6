using UnityEngine;

public class ItemBase<T> : MonoBehaviour where T : ItemDataSO
{
    public T data ;
    public int amount;
}
