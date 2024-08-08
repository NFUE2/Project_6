using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : UIBase
{
    public void GoToMain()
    {
        Hide();
        UIManager.Instance.Show<IntroPopup>();
    }
}
