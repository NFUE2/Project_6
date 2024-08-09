using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : UIBase
{
    //public void OnClickLeaveGame()
    //{
    //    PhotonNetwork.Disconnect();
    //    PhotonNetwork.LoadLevel(0);
    //    SoundManager.instance.ChangeBGM(BGMList.Intro);
    //}
    public void GoToMain()
    {
        Hide();
        UIManager.Instance.Show<IntroPopup>();
    }
}
