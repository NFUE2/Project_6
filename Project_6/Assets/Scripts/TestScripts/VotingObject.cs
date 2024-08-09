using Photon.Pun;
using UnityEngine;
using System;

public enum DestinationType
{
    Town,
    Stage,
    Boss,
}

[Serializable]
public class DestinationData
{
    public DestinationType type;
    public Transform startTransform;
    public Transform bossSpawn;

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
        GameObject g = collision.gameObject;

        if (!GameManager.instance.players.Contains(g)) return;

        playerCount++;
        if (playerCount == curPlayersCount)
        {
            //추후 UI매니저에서 사용해주기
            voting.SetActive(true);
            voting.GetComponent<Voting>().Display(vdata,data);
            //===============
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerCount--;
        voting.SetActive(false);
        voting.GetComponent<Voting>().ResetVote();
    }
}
