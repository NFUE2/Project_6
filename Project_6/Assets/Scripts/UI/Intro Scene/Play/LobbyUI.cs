using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : UIBase
{
    public void GoToMain()
    {
        Hide();
        UIManager.Instance.Show<IntroPopup>();
    }
}
