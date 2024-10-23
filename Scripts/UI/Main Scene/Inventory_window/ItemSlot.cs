using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler,IDropHandler
{
    public Image itemImage;
    public DraggingItem draggingItem;

    //드래그
    #region Drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingItem.slot = this;

        draggingItem.gameObject.SetActive(true);
        draggingItem.itemImage.sprite = itemImage.sprite;
        itemImage.color = new Color(1,1,1,0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggingItem.gameObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemImage.color = Color.white;
        draggingItem.gameObject.SetActive(false);
    }
    #endregion

    public void OnDrop(PointerEventData eventData)
    {
        //현재 아이템을 가지고 있는지 여부
        bool isActive = itemImage.gameObject.activeInHierarchy;

        
        if (!isActive)//아이템 없을때
        {
            itemImage.sprite = draggingItem.itemImage.sprite;
            itemImage.gameObject.SetActive(true);
            draggingItem.Clear();
        }
        else //아이템 있을때
        {
            ItemSlot slot = draggingItem.slot;
            Sprite item = itemImage.sprite;
            
            itemImage.sprite = slot.itemImage.sprite;
            slot.itemImage.sprite = item;
        }
    }

    //비우기
    public void Clear()
    {
        itemImage.gameObject.SetActive(false);
        itemImage.sprite = null;
    }
}
