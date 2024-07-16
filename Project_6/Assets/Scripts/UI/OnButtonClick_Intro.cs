using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnButtonClick_Intro : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;


    public void ColorChange()
    {
        ColorBlock colorBlock = button.colors;

        colorBlock.highlightedColor = new Color(0f, 1f, 0f, 1f);
        colorBlock.pressedColor = new Color(0f, 0.5f, 0f, 1f);

        button.colors = colorBlock;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {


        ColorBlock colorBlock = button.colors;

        colorBlock.highlightedColor = new Color(0f, 1f, 0f, 1f);
        colorBlock.pressedColor = new Color(0f, 0.5f, 0f, 1f);

        button.colors = colorBlock;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
