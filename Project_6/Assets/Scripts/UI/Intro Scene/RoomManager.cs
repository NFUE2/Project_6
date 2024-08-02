using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject startButton;
    public Transform playerList;
    public GameObject playerPrefab;
    public GameObject roomFrame;

    public Button[] buttons;

    private Dictionary<int, GameObject> playerListEntries;

    public void OnClickGameStart()
    {
        photonView.RPC(nameof(ChangeClipRPC), RpcTarget.All);

        foreach(var b in buttons)
            b.interactable = false;

        PhotonNetwork.LoadLevel(1);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        //SoundManager.instance.ChangeBGM(BGMList.Town);
    }

    [PunRPC]
    void ChangeClipRPC()
    {
        SoundManager.instance.ChangeBGM(BGMList.Town);
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient) PhotonNetwork.EnableCloseConnection = true;


        base.OnJoinedRoom();
        roomFrame.SetActive(true);

        foreach (var b in buttons)
            b.interactable = true;

        startButton.SetActive(PhotonNetwork.IsMasterClient);

        if (playerListEntries == null) playerListEntries = new Dictionary<int, GameObject>();

        foreach(Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            GameObject go = Instantiate(playerPrefab);
            go.transform.parent = playerList;
            //go.GetComponentInChildren<TextMeshProUGUI>().text += p.ActorNumber;
            go.GetComponentInChildren<TextMeshProUGUI>().text += PhotonNetwork.CurrentRoom.PlayerCount;

            if (p == PhotonNetwork.LocalPlayer) go.GetComponent<RoomPlayer>().IsMine();

            playerListEntries.Add(p.ActorNumber, go);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        GameObject go = Instantiate(playerPrefab);
        go.transform.parent = playerList;
        //go.GetComponentInChildren<TextMeshProUGUI>().text += newPlayer.ActorNumber;
        go.GetComponentInChildren<TextMeshProUGUI>().text += PhotonNetwork.CurrentRoom.PlayerCount;

        playerListEntries.Add(newPlayer.ActorNumber, go);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        roomFrame.SetActive(false);

        foreach(GameObject go in playerListEntries.Values) Destroy(go);

        playerListEntries.Clear();
        playerListEntries = null;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        base.OnPlayerLeftRoom(otherPlayer);

        if(playerListEntries.TryGetValue(otherPlayer.ActorNumber,out GameObject go))
        {
            Destroy(go);
            playerListEntries.Remove(otherPlayer.ActorNumber);
        }
    }

    public void OnClickLeaveRoom()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = false;

        PhotonNetwork.LeaveRoom();
    }
}
