using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

enum SceneType
{
    intro,
    Main
}

public class P_Network : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI text,count;
    

    private void Awake()
    {
        //서버접속
        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        text.text = PhotonNetwork.NetworkClientState.ToString();
        if (PhotonNetwork.InRoom) count.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }

    public void JoinRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomOrCreateRoom();
        StartCoroutine(SceneChaneCheck(ClientState.Joined,SceneType.Main));
    }

    IEnumerator SceneChaneCheck(ClientState state,SceneType type)
    {
        ClientState curState = PhotonNetwork.NetworkClientState;

        while (curState != state)
        {
            curState = PhotonNetwork.NetworkClientState;
            yield return null;
        }

        PhotonNetwork.LoadLevel((int)type);
    }
}
