using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : PunSingleton<NetworkManager>
{
    public GameObject networkPanel, panelExitButton;//,lobby; //상태변화 안내패널, 닫기버튼
    public TextMeshProUGUI infoText; //한글이 안되서 주석처리, 상태변화 메세지 표시
    //public Text infoText;

    //public TMP_InputField roomInputField; //한글이 안되서 주석처리
    //public TextMeshProUGUI state; //현재 포톤 접속상태 확인, 에디터에서만 사용

    //현재 상태를 나타내는 메세지들
    public readonly string connectServerMessage = "서버에 접속중입니다.";
    public readonly string connectLobbyMessage = "로비에 접속중입니다.";
    //public readonly string refreshLobbyMessage = "새로고침 중입니다."; //로비 매니저로 이관
    //public readonly string connectRoomMessage = "방에 입장중입니다."; //로비 매니저로 이관
    public readonly string createRoomFailMessage = "방 생성에 실패하였습니다.";
    public readonly string joinRoomFailMessage = "방 입장에 실패하였습니다.";

    public string curChoiceRoom;

    public Button[] buttons;

    public override void Awake()
    {
        base.Awake();
//#if UNITY_EDITOR
//        state.enabled = true;
//#endif
    }

    private void Update()
    {
//#if UNITY_EDITOR
//        state.text = PhotonNetwork.NetworkClientState.ToString();
//#endif
    }

    public void OnClickEnterServer()
    {
        foreach (var b in buttons)
            b.interactable = false;

        StartCoroutine(ChangeState(
            connectServerMessage,
            ClientState.ConnectedToMasterServer,
            PhotonNetwork.ConnectUsingSettings));

        //StartCoroutine(ChangeState(
        //    "Server Connecting",
        //    ClientState.ConnectedToMasterServer,
        //    PhotonNetwork.ConnectUsingSettings));

        //Debug.Log(PhotonNetwork.ConnectUsingSettings());
    }

    public void OnJoinLobby()
    {
        StartCoroutine(ChangeState(
           connectServerMessage,
           ClientState.JoinedLobby,
           PhotonNetwork.JoinLobby));

        PhotonNetwork.AutomaticallySyncScene = true;
        //Debug.Log(PhotonNetwork.JoinLobby());
    }

    //public void OnClickRefreshLobby() //로비 매니저로 이관
    //{
    //    StartCoroutine(ChangeState(
    //      refreshLobbyMessage,
    //      ClientState.JoinedLobby,
    //      PhotonNetwork.JoinLobby));
    //}

    //public void OnClickJoinRoom() //로비 매니저로 이관
    //{
    //    StartCoroutine(ChangeState(
    //      connectRoomMessage,
    //      ClientState.Joined,
    //      JoinRoom));
    //}
    
    //네트워크매니저 처리
    //public void OnClickDisconnect()
    //{
    //    PhotonNetwork.Disconnect();
    //}

    //public bool JoinRoom() //로비 매니저로 이관
    //{
    //    return PhotonNetwork.JoinRoom(curChoiceRoom);
    //}

    //포톤에서의 상태 변화
    public IEnumerator ChangeState(string message, ClientState targetState, Func<bool> func)
    {
        if (!func()) yield break; //코루틴 종료

        networkPanel.SetActive(true);
        infoText.text = message;

        ClientState curState = PhotonNetwork.NetworkClientState;

        while (curState != targetState)
        {
            curState = PhotonNetwork.NetworkClientState;
            yield return null;
        }

        networkPanel.SetActive(false);
    }

    //포톤 에러코드 정리, 네트워크 매니저에서 처리
    private string ErrorMessage(short returnCode)
    {
        string message = "";

        switch (returnCode)
        {
            case 32766:
                message = "중복된 방이 존재합니다.";
                break;

            case 32765:
                message = "방이 가득 찼습니다.";
                break;

            case 32764: //정확히는 게임이 종료된 방에 입장시 뜨는 에러, 방이 닫혔을때 뜨는듯
                message = "방에 입장할 수 없습니다.";
                break;

            case 32758:
                message = "방이 존재하지않습니다.";
                break;

            case 32762:
                message = "잠시 후에 시도해주세요";
                break;
        }
        return message;
    }

    //UI에서 처리
    private void FailTextInfo(string message)
    {
        infoText.text = message;
        panelExitButton.SetActive(false);
    }

    #region ServerCallbacks

    //public override void OnConnected()
    //{
    //    base.OnConnected();
    //    Debug.Log("접속 시도");
    //}

    //접속하면 곧바로 로비로 접속 시도
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //Debug.Log("마스터 접속 성공");
        OnJoinLobby(); //마스터서버 접속되면 로비로 접속
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        foreach (var b in buttons)
            b.interactable = true;
    }

    #endregion

    #region LobbyCallbacks
    //로비관련은 로비매니저에서 담당
    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    //lobby.SetActive(true);
    //}

    //public override void OnLeftLobby()
    //{
    //    base.OnLeftLobby();
    //}
    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    base.OnRoomListUpdate(roomList);
    //    //TestLobbyManager.instance.SetRoomList(roomList); //로비 매니저로 이관
    //}

    #endregion

    #region RoomCallbacks

    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //}
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //}

    //public override void OnLeftRoom()
    //{
    //    base.OnLeftRoom();
    //}

    ////다른플레이어가 방에 입장했을때
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    ////다른플레이어가 방을 나갔을때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if (otherPlayer.ActorNumber == 1)
        {
            int index = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            if (index == 1)
            {
                //PhotonNetwork.LoadLevel(0);
                SceneControl.instance.LoadScene(SceneType.Intro);
                SoundManager.instance.ChangeBGM(BGMList.Intro);
            }

            PhotonNetwork.LeaveRoom();
        }
    }

    ////방장 변경되었을때
    //public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    //{
    //    base.OnMasterClientSwitched(newMasterClient);
    //}
    #endregion 

    #region FailsCallbacks 실패시 작동하는 함수들
    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    base.OnJoinRandomFailed(returnCode, message);
    //    Debug.Log(returnCode);
    //}

    //방 생성 실패, 네트워크 매니저에서 처리
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        StopAllCoroutines();
        Debug.Log(returnCode);
        Debug.Log(message);
        FailTextInfo($"{createRoomFailMessage}\n\n{ErrorMessage(returnCode)}");
    }

    //방 입장 실패
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        StopAllCoroutines();
        FailTextInfo($"{joinRoomFailMessage}\n\n{ErrorMessage(returnCode)}");
    }

    #endregion 
}
