using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;


public class P_CharacterButton : MonoBehaviour, IPunObservable
{
    Button button;
    PhotonView pv;

    public GameObject panel;
    public GameObject gameStartButton;

    int count = 0;

    private void Awake()
    {
        button = GetComponent<Button>();
        pv = GetComponent<PhotonView>();
    }

    public void OnClick(GameObject go)
    {
        //��ü ����ȭ
        pv.RPC("OnClickRPC",RpcTarget.MasterClient);
        int playerNum = PhotonNetwork.LocalPlayer.ActorNumber;

        PhotonNetwork.Instantiate(go.name,Vector2.zero + Vector2.right * playerNum,Quaternion.identity);

        panel.SetActive(false);
    }

    [PunRPC]
    private void OnClickRPC()
    {
        button.interactable = false;
        count++;

        if (count >= 4 && PhotonNetwork.IsMasterClient)
            gameStartButton.SetActive(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //Ŭ���̾�Ʈ�� �۵���
        {
            stream.SendNext(button.interactable);
        }
        else //Ŭ���̾�Ʈ �ƴѻ���� �۵���
        {
            button.interactable = (bool)stream.ReceiveNext();
        }
    }
}
