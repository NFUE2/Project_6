using Photon.Pun;
using UnityEngine;
using System;

public enum DestinationType
{
    Town,
    Stage,
}

[Serializable]
public class DestinationData
{
    public DestinationType type;
    public Transform startTransform;

    public bool isCamPlayer;
    public Transform CameraPos;

    public GameObject[] activeGameObject;
    public GameObject[] deactiveGameObject;

    public BGMList bgm;
}

[RequireComponent(typeof(PhotonView))]
public class VotingObject : MonoBehaviourPun//, IPunObservable
{
    int playerCount = 0, curPlayersCount;
    public DestinationData data;
    
    public GameObject voting;
    public VotingDataSO vdata;
    //public TextMeshProUGUI text;
    //public Transform bossStart,bossCamera;
    //public GameObject boss;
    private void Start()
    {
        if (data.type == DestinationType.Town)
        {
            var d = GameManager.instance.town;

            data.startTransform = d.startTransform;
            data.CameraPos = d.CameraPos;
        }

        curPlayersCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (voting == null) voting = GameManager.instance.vote;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerCount++;
        if (playerCount == curPlayersCount)
        {
            voting.SetActive(true);
            voting.GetComponent<Voting>().Display(vdata,data);
        }

        //photonView.RPC(nameof(VotingRPC),RpcTarget.AllBuffered);
        //playerCount++;
        //photonView.RPC(nameof(EnterRPC),RpcTarget.All);
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerCount--;
        voting.SetActive(false);

        //playerCount--;
        //photonView.RPC(nameof(ExitRPC), RpcTarget.All);
        //VoteReset();
    }

    //[PunRPC]
    //void EnterRPC()
    //{
    //    playerCount++;
    //    votingObject.SetActive(playerCount == curPlayersCount);
    //}

    //[PunRPC]
    //void ExitRPC()
    //{
    //    playerCount--;
    //    votingObject.SetActive(false);
    //}

    //public void VoteAgreement()
    //{
    //    photonView.RPC(nameof(VoteAgreementRPC),RpcTarget.All);
    //}

    //[PunRPC]
    //void VoteAgreementRPC()
    //{
    //    agree++;
    //    text.text = agree.ToString();

    //    if (agree == curPlayersCount)
    //    {
    //        //photonView.RPC(nameof(EnterBossRoomRPC),RpcTarget.MasterClient);
    //        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TestCameraController>().target = bossCamera;
    //        GameManager.instance.player.transform.position = data.startTransform.position;
    //        GameManager.instance.cam.target = data.CameraPos;

    //        if(data.bossManager != null) data.bossManager.SetActive(true);
    //    }
    //}

    //public void VoteOpposition()
    //{
    //    photonView.RPC(nameof(VoteOppositionRPC), RpcTarget.All);
    //}

    //[PunRPC]
    //void VoteOppositionRPC()
    //{
    //    VoteReset();
    //}

    //void VoteReset()
    //{
    //    agree = 0;
    //    votingObject.SetActive(false);
    //}

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
