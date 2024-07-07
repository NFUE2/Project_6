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

    public GameObject networkPanel,panelExitButton; //���º�ȭ �ȳ��г�, �ݱ��ư
    //public TextMeshProUGUI infoText; //�ѱ��� �ȵǼ� �ּ�ó��, ���º�ȭ �޼��� ǥ��
    public Text infoText; //

    //public TMP_InputField roomInputField; //�ѱ��� �ȵǼ� �ּ�ó��
    public TextMeshProUGUI state; //���� ���� ���ӻ��� Ȯ��

    //���� ���¸� ��Ÿ���� �޼�����
    private readonly string connectServerMessage = "������ �������Դϴ�.";
    private readonly string connectLobbyMessage = "�κ� �������Դϴ�.";
    private readonly string connectRoomMessage = "�濡 �������Դϴ�.";
    private readonly string createRoomFailMessage = "�� ������ �����Ͽ����ϴ�.";
    private readonly string joinRoomFailMessage = "�� ���忡 �����Ͽ����ϴ�.";

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

    //���濡���� ���� ��ȭ
    IEnumerator ChangeState(string message, ClientState targetState, Func<bool> func)
    {
        if (!func()) yield break; //�ڷ�ƾ ����

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

    //���� �����ڵ� ����
    private string ErrorMessage(short returnCode)
    {
        string message = "";

        switch (returnCode)
        {
            case 32766:
                message = "�ߺ��� ID�� �����մϴ�.";
                break;

            case 32765:
                message = "���� ���� á���ϴ�.";
                break;

            case 32764: //��Ȯ���� ������ ����� �濡 ����� �ߴ� ����
                message = "���� ã�� �� �����ϴ�.";
                break;

            case 32758:
                message = "���� ���������ʽ��ϴ�.";
                break;

            case 32762:
                message = "��� �Ŀ� �õ����ּ���";
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
    //    //Debug.Log("������ ���� �õ�");
    //}
    //public override void OnConnectedToMaster()
    //{
    //    base.OnConnectedToMaster();
    //    //Debug.Log("������ ���� ����");
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

    //�ٸ��÷��̾ �濡 ����������
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    //�ٸ��÷��̾ ���� ��������
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    //���� ����Ǿ�����
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }
    #endregion 

    #region FailsCallbacks ���н� �۵��ϴ� �Լ���
    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    base.OnJoinRandomFailed(returnCode, message);
    //    Debug.Log(returnCode);
    //}

    //�� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        StopAllCoroutines();
        FailTextInfo($"{createRoomFailMessage}\n\n{ErrorMessage(returnCode)}");
    }

    //�� ���� ����
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        StopAllCoroutines();
        FailTextInfo($"{joinRoomFailMessage}\n\n{ErrorMessage(returnCode)}");
    }
    #endregion 
}
