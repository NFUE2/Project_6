using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Unity.VisualScripting;

[RequireComponent(typeof(PhotonView))]
public class Voting : MonoBehaviourPun
{
    public TextMeshProUGUI message;
    public TextMeshProUGUI agreeText;
    public TextMeshProUGUI oppositeText;
    public TextMeshProUGUI count;

    DestinationData data;
    int agree = 0,playerCount;

    public Button[] buttons;

    private void Awake()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    private void OnEnable()
    {
        ResetButton(true);
        GameManager.instance.player.GetComponent<PlayerInput>().enabled = false;
    }

    private void OnDisable()
    {
        GameManager.instance.player.GetComponent<PlayerInput>().enabled = true;
    }

    void ResetButton(bool active)
    {
        foreach (Button b in buttons)
            b.interactable = active;
    }

    public void Display(VotingDataSO data,DestinationData data2)
    {
        message.text = data.votingMessage;
        agreeText.text = data.agreeText;
        oppositeText.text = data.oppositeText;
        this.data = data2;
    }

    public void Agree()
    {
        ResetButton(false);
        photonView.RPC(nameof(AgreeRPC), RpcTarget.All);
    }

    public void Opposite()
    {
        photonView.RPC(nameof(OppositeRPC),RpcTarget.All);
    }

    [PunRPC]
    public void AgreeRPC()
    {
        agree++;
        count.text = agree.ToString();
        if (agree == playerCount) VotingEnd();
    }

    [PunRPC]
    public void OppositeRPC()
    {
        ResetVote();
        gameObject.SetActive(false);
    }


    void VotingEnd()
    {
        ResetVote();

        GameObject player = GameManager.instance.player;
        player.transform.position = data.startTransform.position;

        if (data.isCamPlayer) GameManager.instance.cam.target = player.transform;
        else GameManager.instance.cam.target = data.CameraPos;

        foreach(var g in data.activeGameObject)
            g.SetActive(true);

        foreach (var g in data.deactiveGameObject)
            g.SetActive(false);

        if(PhotonNetwork.IsMasterClient && data.type == DestinationType.Boss)
        {
            BossBattleManager.instance.SpawnBossMonster(GameManager.instance.cleaStageCount, data.bossSpawn.position);
        }

        SoundManager.instance.ChangeBGM(data.bgm);
    }

    public void ResetVote()
    {
        agree = 0;
        count.text = agree.ToString();
    }
}
