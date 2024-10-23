using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour, IDropHandler,IPointerEnterHandler
{
    public DraggingItem draggingItem;

    //아이템 삭제
    public void OnDrop(PointerEventData eventData)
    {
        draggingItem.Clear();
    }

    //아이템이 드래깅중 쓰레기통위로 왔을때
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(draggingItem.gameObject.activeSelf)
        {

        }
    }
}
