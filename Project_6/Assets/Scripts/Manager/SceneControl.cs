using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    Intro,
    Loading,
    Main,
    Outro,
}

public class SceneControl : Singleton<SceneControl>
{
    public void LoadScene(SceneType scene)
    {
        PhotonNetwork.LoadLevel((int)scene);
    }
}
