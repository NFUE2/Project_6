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


    private void Awake()
    {
        button = GetComponent<Button>();
        pv = GetComponent<PhotonView>();
    }

    public void OnClick(GameObject go)
    {
        //��ü ����ȭ
        pv.RPC("OnClickRPC",RpcTarget.MasterClient);
        P_BossNetwork.instance.PlusCount();
        int playerNum = PhotonNetwork.LocalPlayer.ActorNumber;

        PhotonNetwork.Instantiate(go.name,Vector2.zero + Vector2.right * playerNum,Quaternion.identity);

        panel.SetActive(false);
    }

    [PunRPC]
    private void OnClickRPC()
    {
        button.interactable = false;
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
