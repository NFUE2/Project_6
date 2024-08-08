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
    public GameObject networkPanel, panelExitButton;//,lobby; //���º�ȭ �ȳ��г�, �ݱ��ư
    public TextMeshProUGUI infoText; //�ѱ��� �ȵǼ� �ּ�ó��, ���º�ȭ �޼��� ǥ��
    //public Text infoText;

    //public TMP_InputField roomInputField; //�ѱ��� �ȵǼ� �ּ�ó��
    //public TextMeshProUGUI state; //���� ���� ���ӻ��� Ȯ��, �����Ϳ����� ���

    //���� ���¸� ��Ÿ���� �޼�����
    public readonly string connectServerMessage = "������ �������Դϴ�.";
    public readonly string connectLobbyMessage = "�κ� �������Դϴ�.";
    //public readonly string refreshLobbyMessage = "���ΰ�ħ ���Դϴ�."; //�κ� �Ŵ����� �̰�
    //public readonly string connectRoomMessage = "�濡 �������Դϴ�."; //�κ� �Ŵ����� �̰�
    public readonly string createRoomFailMessage = "�� ������ �����Ͽ����ϴ�.";
    public readonly string joinRoomFailMessage = "�� ���忡 �����Ͽ����ϴ�.";

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

    //public void OnClickRefreshLobby() //�κ� �Ŵ����� �̰�
    //{
    //    StartCoroutine(ChangeState(
    //      refreshLobbyMessage,
    //      ClientState.JoinedLobby,
    //      PhotonNetwork.JoinLobby));
    //}

    //public void OnClickJoinRoom() //�κ� �Ŵ����� �̰�
    //{
    //    StartCoroutine(ChangeState(
    //      connectRoomMessage,
    //      ClientState.Joined,
    //      JoinRoom));
    //}
    
    //��Ʈ��ũ�Ŵ��� ó��
    //public void OnClickDisconnect()
    //{
    //    PhotonNetwork.Disconnect();
    //}

    //public bool JoinRoom() //�κ� �Ŵ����� �̰�
    //{
    //    return PhotonNetwork.JoinRoom(curChoiceRoom);
    //}

    //���濡���� ���� ��ȭ
    public IEnumerator ChangeState(string message, ClientState targetState, Func<bool> func)
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

    //���� �����ڵ� ����, ��Ʈ��ũ �Ŵ������� ó��
    private string ErrorMessage(short returnCode)
    {
        string message = "";

        switch (returnCode)
        {
            case 32766:
                message = "�ߺ��� ���� �����մϴ�.";
                break;

            case 32765:
                message = "���� ���� á���ϴ�.";
                break;

            case 32764: //��Ȯ���� ������ ����� �濡 ����� �ߴ� ����, ���� �������� �ߴµ�
                message = "�濡 ������ �� �����ϴ�.";
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

    //UI���� ó��
    private void FailTextInfo(string message)
    {
        infoText.text = message;
        panelExitButton.SetActive(false);
    }

    #region ServerCallbacks

    //public override void OnConnected()
    //{
    //    base.OnConnected();
    //    Debug.Log("���� �õ�");
    //}

    //�����ϸ� ��ٷ� �κ�� ���� �õ�
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //Debug.Log("������ ���� ����");
        OnJoinLobby(); //�����ͼ��� ���ӵǸ� �κ�� ����
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        foreach (var b in buttons)
            b.interactable = true;
    }

    #endregion

    #region LobbyCallbacks
    //�κ������ �κ�Ŵ������� ���
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
    //    //TestLobbyManager.instance.SetRoomList(roomList); //�κ� �Ŵ����� �̰�
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

    ////�ٸ��÷��̾ �濡 ����������
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    ////�ٸ��÷��̾ ���� ��������
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

    ////���� ����Ǿ�����
    //public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    //{
    //    base.OnMasterClientSwitched(newMasterClient);
    //}
    #endregion 

    #region FailsCallbacks ���н� �۵��ϴ� �Լ���
    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    base.OnJoinRandomFailed(returnCode, message);
    //    Debug.Log(returnCode);
    //}

    //�� ���� ����, ��Ʈ��ũ �Ŵ������� ó��
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        StopAllCoroutines();
        Debug.Log(returnCode);
        Debug.Log(message);
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
