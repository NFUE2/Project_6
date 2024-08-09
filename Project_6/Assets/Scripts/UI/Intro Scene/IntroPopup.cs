using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroPopup : UIBase
{
    public Button[] buttons;
    public GameObject networkPanel, panelExitButton;
    public TextMeshProUGUI infoText;

    private void SetNetworkManager()
    {
        NetworkManager.Instance.networkPanel = networkPanel;
        NetworkManager.Instance.panelExitButton = panelExitButton;
        NetworkManager.Instance.infoText = infoText;
        NetworkManager.Instance.buttons = buttons;
    }

    public void OnClickButton(string str)
    {
        switch(str)
        {
            case "Play":
                SetNetworkManager();
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
