using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class P_Network : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI text;
    public GameObject startBtn;

    private void Awake()
    {
        //서버접속
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update() => text.text = PhotonNetwork.NetworkClientState.ToString();

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            startBtn.SetActive(false);
        }
    }
}
