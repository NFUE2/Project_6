using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DraggingItem : MonoBehaviour
{
    public Image itemImage;
    [HideInInspector] public ItemSlot slot;

    public void Clear()
    {
        slot.Clear();
    }
}
