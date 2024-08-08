using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
    public void OnClickLeaveGame()
    {
        PhotonNetwork.Disconnect();
        //PhotonNetwork.LoadLevel(0);
        SceneControl.instance.LoadScene(SceneType.Intro);
        SoundManager.instance.ChangeBGM(BGMList.Intro);
    }
}
