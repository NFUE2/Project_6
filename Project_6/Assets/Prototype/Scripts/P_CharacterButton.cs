using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;


public class P_CharacterButton : MonoBehaviourPun, IPunObservable
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
        pv.RPC("OnClickRPC",RpcTarget.AllBuffered);
        P_BossNetwork.instance.PlusCount();
        int playerNum = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject gobj = PhotonNetwork.Instantiate("Prototype/" + go.name, Vector2.zero + Vector2.right * playerNum, Quaternion.identity);

        panel.SetActive(false);
    }

    [PunRPC]
    private void OnClickRPC()
    {
        button.interactable = false;
        //int playerNum = PhotonNetwork.LocalPlayer.ActorNumber;
        //PhotonNetwork.Instantiate(name, Vector2.zero + Vector2.right * playerNum, Quaternion.identity);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //Ŭ���̾�Ʈ�� �۵���
        {
            stream.SendNext(button.interactable);
            //Debug.Log(1);
        }
        else //Ŭ���̾�Ʈ �ƴѻ���� �۵���
        {
            button.interactable = (bool)stream.ReceiveNext();
            //Debug.Log(button.interactable);
        }
    }
}
