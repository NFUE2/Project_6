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
    [Header("��")]
    [Tooltip("�渮��Ʈ ǥ�� ��ġ")]
    public Transform roomContent;

    [Tooltip("�� ������")]
    public GameObject roomPrefab;

    public TMP_InputField roomInputField; //�ѱ��� �ȵǼ� �ּ�ó��
    public GameObject lobbyFrame;

    public readonly string refreshLobbyMessage = "���ΰ�ħ ���Դϴ�.";
    public readonly string connectRoomMessage = "�濡 �������Դϴ�.";

    public string choiceRoomName = string.Empty;

    public TMP_InputField textMeshPro;

    public override void OnJoinedLobby() //�κ� ����
    {
        base.OnJoinedLobby();
        lobbyFrame.SetActive(true);
    }

    public override void OnJoinedRoom() //�� ������
    {
        base.OnJoinedRoom();
        lobbyFrame.SetActive(false);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList) 
    {
        base.OnRoomListUpdate(roomList);
        SetRoomList(roomList);
    }
    public void OnClickJoinRoom() //�� �����ư Ŭ��
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

    public bool JoinRoom() //�� ����
    {
        return PhotonNetwork.JoinRoom(choiceRoomName);
    }

    public void SetRoomList(List<RoomInfo> roomList) //�� ��� ����
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

    public void OnClickRefreshLobby() //����ΰ�ħ
    {
        StartCoroutine(NetworkManager.instance.ChangeState(
          refreshLobbyMessage,
          ClientState.JoinedLobby,
          PhotonNetwork.JoinLobby));
    }

    private void RoomListClear() //���� ����
    {
        foreach (Transform child in roomContent)
            Destroy(child.gameObject);
    }

    public void OnCreateRoom(GameObject panel) //�游���
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
