using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Dispenser : MonoBehaviour,IPunInstantiateMagicCallback
{
    private float targetTime = 5f;
    private float curTime = 0;
    //PhotonView pv;

    //private void Start()
    //{
    //    pv = GetComponent<PhotonView>();

    //}

    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= targetTime)
        {
            //Destroy(gameObject);
            DestroyObject();
            //(º¹¿ø)pv.RPC("DestroyObject", RpcTarget.All);
        }
    }
    [PunRPC]
    private void DestroyObject()
    {
        //PhotonNetwork.Destroy(gameObject);
        Destroy(gameObject);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);
    }
}
