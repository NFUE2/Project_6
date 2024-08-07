using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Scene
{
    Intro,
    Loading,
    Main,
    Outro,
}

public class SceneManager : Singleton<SceneManager>
{
    public void LoadScene(Scene scene)
    {
        PhotonNetwork.LoadLevel((int)scene);
    }
}
