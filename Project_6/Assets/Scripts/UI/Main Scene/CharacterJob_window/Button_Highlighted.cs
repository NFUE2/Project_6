using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Highlighted : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    //public Image image;


    public void OnPointerEnter(PointerEventData eventData)
    {


        //image.color = new Color(1f, 0f, 0f, 1f);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //image.color = new Color(1f, 0f, 0f, 1f);
    }
}
