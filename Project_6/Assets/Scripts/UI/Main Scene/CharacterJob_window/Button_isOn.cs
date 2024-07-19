using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_isOn : MonoBehaviour
{
    public bool ButtonIsOn = false;

    public void OnClick()
    {
        ButtonIsOn = !ButtonIsOn;
    }
}
