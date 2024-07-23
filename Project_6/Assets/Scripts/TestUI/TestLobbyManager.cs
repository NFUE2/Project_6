using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class TestLobbyManager : MonoBehaviourPunCallbacks
{
    [Header("방")]
    [Tooltip("방리스트 표기 위치")]
    public Transform roomContent;

    [Tooltip("방 프리펩")]
    public GameObject roomPrefab;

    public TMP_InputField roomInputField; //한글이 안되서 주석처리
    public GameObject lobbyFrame;

    public readonly string refreshLobbyMessage = "새로고침 중입니다.";
    public readonly string connectRoomMessage = "방에 입장중입니다.";

    public string choiceRoomName = string.Empty;

    public TMP_InputField textMeshPro;

    public override void OnJoinedLobby() //로비에 입장
    {
        base.OnJoinedLobby();
        lobbyFrame.SetActive(true);
    }

    public override void OnJoinedRoom() //방 들어갔을때
    {
        base.OnJoinedRoom();
        lobbyFrame.SetActive(false);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList) 
    {
        base.OnRoomListUpdate(roomList);
        SetRoomList(roomList);
    }
    public void OnClickJoinRoom() //방 입장버튼 클릭
    {
        if (choiceRoomName.Length == 0) return;

        StartCoroutine(NetworkManager.instance.ChangeState(
          connectRoomMessage,
          ClientState.Joined,
          JoinRoom));
    }
    public void OnClickDisconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public bool JoinRoom() //방 입장
    {
        return PhotonNetwork.JoinRoom(choiceRoomName);
    }

    public void SetRoomList(List<RoomInfo> roomList) //방 목록 생성
    {
        RoomListClear();
        
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.PlayerCount == 0) continue;

            TestLobbyRoom room = Instantiate(roomPrefab,roomContent).GetComponent<TestLobbyRoom>();
            room.SetRoomInfo(roomInfo);
            room.lobbyManager = this;
        }
    }

    public void OnClickRefreshLobby() //방새로고침
    {
        StartCoroutine(NetworkManager.instance.ChangeState(
          refreshLobbyMessage,
          ClientState.JoinedLobby,
          PhotonNetwork.JoinLobby));
    }

    private void RoomListClear() //방목록 제거
    {
        foreach (Transform child in roomContent)
            Destroy(child.gameObject);
    }

    public void OnCreateRoom(GameObject panel) //방만들기
    {
        
        if(textMeshPro.text != "" || textMeshPro.text != null)
        {
            panel.SetActive(false);

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(roomInputField.text, roomOptions);
        }

    }
}
