using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroPopup : UIBase
{
    public Button[] buttons;

    private void SendButtons()
    {
        NetworkManager.Instance.buttons = buttons;
    }

    public void OnClickButton(string str)
    {
        switch(str)
        {
            case "Play":
                SendButtons();
                NetworkManager.Instance.OnClickEnterServer();
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
