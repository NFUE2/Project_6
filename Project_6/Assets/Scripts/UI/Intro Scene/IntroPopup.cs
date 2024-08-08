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
                UIManager.Instance.Show<LobbyUI>();
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
