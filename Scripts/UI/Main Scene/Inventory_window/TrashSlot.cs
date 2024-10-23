using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour, IDropHandler,IPointerEnterHandler
{
    public DraggingItem draggingItem;

    //������ ����
    public void OnDrop(PointerEventData eventData)
    {
        draggingItem.Clear();
    }

    //�������� �巡���� ������������ ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(draggingItem.gameObject.activeSelf)
        {

        }
    }
}
