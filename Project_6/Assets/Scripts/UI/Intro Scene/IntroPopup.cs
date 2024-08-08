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
            case "Tutorial":
                UIManager.Instance.Show<TutorialUI>();
                break;
        }
    }
}
