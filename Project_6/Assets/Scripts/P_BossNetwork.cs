using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_BossNetwork : MonoBehaviour,IPunObservable
{
    PhotonView pv;
    public Image image;
    public GameObject boss;

    public static P_BossNetwork instance;
    public int count = 0;
    public GameObject gamestartButton;

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
            gamestartButton.SetActive(true);
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(image.fillAmount);
        }
        else
        {
            image.fillAmount = (float)stream.ReceiveNext();
        }
    }

    public void BossActive()
    {
        pv.RPC("BossActiveRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void BossActiveRPC()
    {
        PhotonNetwork.Instantiate(boss.name,new Vector3(0,1,0),Quaternion.identity);
        //boss.SetActive(true);
        gamestartButton.SetActive(false);
    }

    public void PlusCount()
    {
        //count++;
        //if (count >= 4)
        //    gamestartButton.SetActive(true);
    }

}
