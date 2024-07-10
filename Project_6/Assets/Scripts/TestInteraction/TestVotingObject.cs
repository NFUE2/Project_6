using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PhotonView))]
public class TestVotingObject : MonoBehaviourPun, IPunObservable
{
    int playerCount = 0,curPlayersCount, agree = 0;

    public GameObject votingObject;
    public TextMeshProUGUI text;
    public Transform bossStart,bossCamera;
    public GameObject boss;

    private void Start()
    {
        curPlayersCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log(curPlayersCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //photonView.RPC(nameof(VotingRPC),RpcTarget.AllBuffered);
        playerCount++;
        votingObject.SetActive(playerCount == curPlayersCount);
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerCount--;
        VoteReset();
    }

    public void VoteAgreement()
    {
        agree++;
        text.text = agree.ToString();
        if(agree == curPlayersCount)
        {
            photonView.RPC(nameof(EnterBossRoomRPC),RpcTarget.MasterClient);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TestCameraController>().target = bossCamera;
        }
    }

    public void VoteOpposition()
    {
        VoteReset();
    }

    void VoteReset()
    {
        agree = 0;
        votingObject.SetActive(false);
    }

    [PunRPC]
    void EnterBossRoomRPC()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject p in players)
            p.transform.position = bossStart.position;

        boss.SetActive(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(agree);
            stream.SendNext(playerCount);
            stream.SendNext(votingObject.activeInHierarchy);
        }
        else
        {
            agree = (int)stream.ReceiveNext();
            playerCount = (int)stream.ReceiveNext();
            votingObject.SetActive((bool)stream.ReceiveNext());
        }
    }

    //[PunRPC]
    //void VotingRPC()
    //{
    //    playerCount++;
    //    votingObject.SetActive(playerCount == curPlayersCount);
    //}

    //[PunRPC]
    //void RunRPC()
    //{
    //    playerCount++;
    //    votingObject.SetActive(playerCount == curPlayersCount);
    //}
}
