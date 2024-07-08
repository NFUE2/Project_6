using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject startButton;
    public Transform playerList;
    public GameObject playerPrefab;
    public GameObject roomFrame;

    private GameObject curPlayer;

    private Dictionary<int, GameObject> playerListEntries;

    private void Awake()
    {
    }

    public void OnClickGameStart()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomFrame.SetActive(true);

        if (PhotonNetwork.IsMasterClient) startButton.SetActive(true);

        if (playerListEntries == null) playerListEntries = new Dictionary<int, GameObject>();

        foreach(Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            GameObject go = Instantiate(playerPrefab);
            go.transform.parent = playerList;
            go.GetComponentInChildren<TextMeshProUGUI>().text += p.ActorNumber;

            playerListEntries.Add(p.ActorNumber, go);
        }

        //curPlayer = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        //curPlayer.transform.parent = 
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        GameObject go = Instantiate(playerPrefab);
        go.transform.parent = playerList;
        go.GetComponentInChildren<TextMeshProUGUI>().text += newPlayer.ActorNumber;

        playerListEntries.Add(newPlayer.ActorNumber, go);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        roomFrame.SetActive(false);

        foreach(GameObject go in playerListEntries.Values)
            Destroy(go);

        playerListEntries.Clear();
        playerListEntries = null;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
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
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 1)
            PhotonNetwork.CurrentRoom.IsOpen = false;

        PhotonNetwork.LeaveRoom();
    }

    
}
