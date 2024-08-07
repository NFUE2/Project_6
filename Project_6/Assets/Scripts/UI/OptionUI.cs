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
        SceneManager.instance.LoadScene(Scene.Intro);
        SoundManager.instance.ChangeBGM(BGMList.Intro);
    }
}
