using Photon.Pun;
using UnityEngine;
using TMPro;
using System;
using JetBrains.Annotations;

[Serializable]
public class BossBattleData
{
    public Transform bossStart;
    public Transform bossBattleCameraPos;
    public GameObject bossManager;
}

[RequireComponent(typeof(PhotonView))]
public class VotingObject : MonoBehaviourPun//, IPunObservable
{
    int playerCount = 0,curPlayersCount, agree = 0;

    public GameObject votingObject;
    public TextMeshProUGUI text;
    //public Transform bossStart,bossCamera;
    //public GameObject boss;
    public BossBattleData data;
    public GameObject enemyList;

    private void Start()
    {
        curPlayersCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //photonView.RPC(nameof(VotingRPC),RpcTarget.AllBuffered);
        //playerCount++;
        photonView.RPC(nameof(EnterRPC),RpcTarget.All);
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        //playerCount--;
        photonView.RPC(nameof(ExitRPC), RpcTarget.All);
        VoteReset();
    }

    [PunRPC]
    void EnterRPC()
    {
        playerCount++;
        votingObject.SetActive(playerCount == curPlayersCount);
    }

    [PunRPC]
    void ExitRPC()
    {
        playerCount--;
        votingObject.SetActive(false);
    }

    public void VoteAgreement()
    {
        Debug.Log(1);
        photonView.RPC(nameof(VoteAgreementRPC),RpcTarget.All);
    }

    [PunRPC]
    void VoteAgreementRPC()
    {
        agree++;
        text.text = agree.ToString();

        if (agree == curPlayersCount)
        {
            Debug.Log("¿Ãµø");

            //photonView.RPC(nameof(EnterBossRoomRPC),RpcTarget.MasterClient);
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TestCameraController>().target = bossCamera;
            GameManager.instance.player.transform.position = data.bossStart.position;
            GameManager.instance.cam.target = data.bossBattleCameraPos;
            data.bossManager.SetActive(true);
            enemyList.SetActive(false);
        }
    }

    public void VoteOpposition()
    {
        photonView.RPC(nameof(VoteOppositionRPC), RpcTarget.All);

    }

    [PunRPC]
    void VoteOppositionRPC()
    {
        VoteReset();
    }

    void VoteReset()
    {
        agree = 0;
        votingObject.SetActive(false);
    }

    //[PunRPC]
    //void EnterBossRoomRPC()
    //{
    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    //    foreach(GameObject p in players)
    //        p.transform.position = bossStart.position;

    //    boss.SetActive(true);
    //}

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if(stream.IsWriting)
    //    {
    //        stream.SendNext(agree);
    //        stream.SendNext(playerCount);
    //        stream.SendNext(votingObject.activeInHierarchy);
    //    }
    //    else
    //    {
    //        agree = (int)stream.ReceiveNext();
    //        playerCount = (int)stream.ReceiveNext();
    //        votingObject.SetActive((bool)stream.ReceiveNext());
    //    }
    //}

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
