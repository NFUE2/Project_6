using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class P_CharacterButton : MonoBehaviour, IPunObservable
{
    Button button;
    PhotonView pv;

    GameObject character;

    private void Awake()
    {
        button = GetComponent<Button>();
        pv = GetComponent<PhotonView>();
    }

    public void OnClick(GameObject go)
    {
        //전체 동기화
        pv.RPC("OnClickRPC",RpcTarget.MasterClient);
    }

    [PunRPC]
    private void OnClickRPC()
    {
        button.interactable = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //클라이언트만 작동함
        {
            stream.SendNext(button.interactable);
        }
        else //클라이언트 아닌사람만 작동함
        {
            button.interactable = (bool)stream.ReceiveNext();
        }
    }
}
