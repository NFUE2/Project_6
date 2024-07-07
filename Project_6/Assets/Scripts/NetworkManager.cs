using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public NetworkManager instance;

    public GameObject networkPanel,panelExitButton; //상태변화 안내패널, 닫기버튼
    //public TextMeshProUGUI infoText; //한글이 안되서 주석처리, 상태변화 메세지 표시
    public Text infoText; //

    //public TMP_InputField roomInputField; //한글이 안되서 주석처리
    public TextMeshProUGUI state; //현재 포톤 접속상태 확인

    //현재 상태를 나타내는 메세지들
    private readonly string connectServerMessage = "서버에 접속중입니다.";
    private readonly string connectLobbyMessage = "로비에 접속중입니다.";
    private readonly string connectRoomMessage = "방에 입장중입니다.";
    private readonly string createRoomFailMessage = "방 생성에 실패하였습니다.";
    private readonly string joinRoomFailMessage = "방 입장에 실패하였습니다.";

    public string curChoiceRoom;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        state.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void OnClickEnterServer()
    {
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

    public void OnClickJoinLobby()
    {
        StartCoroutine(ChangeState(
           connectServerMessage,
           ClientState.JoinedLobby,
           PhotonNetwork.JoinLobby));

        //Debug.Log(PhotonNetwork.JoinLobby());
    }


    public void OnClickJoinRoom()
    {
        StartCoroutine(ChangeState(
          connectRoomMessage,
          ClientState.Joined,
          JoinRoom));
    }

    public bool JoinRoom()
    {
        return PhotonNetwork.JoinRoom(curChoiceRoom);
    }

    public void OnCreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(roomName,roomOptions);
    }

    //포톤에서의 상태 변화
    IEnumerator ChangeState(string message, ClientState targetState, Func<bool> func)
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

    //포톤 에러코드 정리
    private string ErrorMessage(short returnCode)
    {
        string message = "";

        switch (returnCode)
        {
            case 32766:
                message = "중복된 ID가 존재합니다.";
                break;

            case 32765:
                message = "방이 가득 찼습니다.";
                break;

            case 32764: //정확히는 게임이 종료된 방에 입장시 뜨는 에러
                message = "방을 찾을 수 없습니다.";
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

    private void FailTextInfo(string message)
    {
        infoText.text = message;
        panelExitButton.SetActive(false);
    }

    #region ServerCallbacks

    //public override void OnConnected()
    //{
    //    base.OnConnected();
    //    //Debug.Log("마스터 접속 시도");
    //}
    //public override void OnConnectedToMaster()
    //{
    //    base.OnConnectedToMaster();
    //    //Debug.Log("마스터 접속 성공");
    //}
    #endregion

    #region LobbyCallbacks
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
    }

    #endregion

    #region RoomCallbacks

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }

    //다른플레이어가 방에 입장했을때
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    //다른플레이어가 방을 나갔을때
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    //방장 변경되었을때
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }
    #endregion 

    #region FailsCallbacks 실패시 작동하는 함수들
    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    base.OnJoinRandomFailed(returnCode, message);
    //    Debug.Log(returnCode);
    //}

    //방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        StopAllCoroutines();
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
