using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Voting : MonoBehaviourPun
{
    public TextMeshProUGUI message;
    public TextMeshProUGUI agreeText;
    public TextMeshProUGUI oppositeText;
    public TextMeshProUGUI count;

    int agree = 0;

    public void Display()
    {

    }

    public void Agree()
    {
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
    }

    [PunRPC]
    public void OppositeRPC()
    {
        gameObject.SetActive(false);
        agree = 0;
    }
}
