using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPopup : UIBase
{
    public void OnClickButton(string str)
    {
        switch(str)
        {
            case "Play":
                break;
            case "Option":
                UIManager.Instance.Show<OptionUI>();
                break;
            case "Exit":
                Debug.Log("게임 종료");
                break;
        }
    }
}
