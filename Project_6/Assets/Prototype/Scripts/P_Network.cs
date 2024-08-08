using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

<<<<<<< Updated upstream
enum SceneType
{
    intro,
    Main
}
=======
//enum Scenename
//{
//    intro,
//    Main
//}
>>>>>>> Stashed changes

//public class P_Network : MonoBehaviourPunCallbacks
//{
//    public TextMeshProUGUI text,count;
    

//    private void Awake()
//    {
//        //서버접속
//        PhotonNetwork.AutomaticallySyncScene = true;
//        PhotonNetwork.ConnectUsingSettings();
//        DontDestroyOnLoad(gameObject);
//        Application.targetFrameRate = 60;
//    }

//    //public override void OnPlayerEnteredRoom(Player newPlayer)
//    //{
//    //    base.OnPlayerEnteredRoom(newPlayer);
//    //}

//    public override void OnCreatedRoom()
//    {
//        //Debug.Log("방생성 : " + PhotonNetwork.CurrentRoom.Name);
//    }

//    public override void OnJoinedRoom()
//    {
//        //Debug.Log("방입장 : " + PhotonNetwork.CurrentRoom.Name);
//    }
//    public override void OnJoinRandomFailed(short returnCode, string message)
//    {
//        //Debug.Log("방입장 실패 : Random");
//    }
//    public override void OnJoinRoomFailed(short returnCode, string message)
//    {
//        //Debug.Log("방입장 실패 message : " + message);
//    }

//    private void Update()
//    {
//        text.text = PhotonNetwork.NetworkClientState.ToString();
//        if (PhotonNetwork.InRoom) count.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
//    }

<<<<<<< Updated upstream
    public void JoinRoom()
    {
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, $"Test", options);
        //PhotonNetwork.JoinRandomOrCreateRoom();
        StartCoroutine(SceneChaneCheck(ClientState.Joined,SceneType.Main));
    }

    IEnumerator SceneChaneCheck(ClientState state,SceneType target)
    {
        ClientState curState = PhotonNetwork.NetworkClientState;
=======
//    public void JoinRoom()
//    {
//        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
//        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, $"Test", options);
//        //PhotonNetwork.JoinRandomOrCreateRoom();
//        StartCoroutine(SceneChaneCheck(ClientState.Joined,Scenename.Main));
//    }

//    IEnumerator SceneChaneCheck(ClientState state, Scenename target)
//    {
//        ClientState curState = PhotonNetwork.NetworkClientState;
>>>>>>> Stashed changes

//        while (curState != state)
//        {
//            curState = PhotonNetwork.NetworkClientState;
//            yield return null;
//        }

<<<<<<< Updated upstream
        PhotonNetwork.LoadLevel(/*(int)target*/1);
    }
}
=======
//        //PhotonNetwork.LoadLevel(/*(int)target*/1);
//    }
//}
>>>>>>> Stashed changes
