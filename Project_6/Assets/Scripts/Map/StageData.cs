using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterSpawn
{
    public Transform transform;
    public GameObject monster;
}

public class StageData : MonoBehaviourPun,IPunInstantiateMagicCallback
{
    public MonsterSpawn[] monsterSpawn;
    public Transform monsterList;
    public DestinationData data;
    public List<GameObject> monsters = new List<GameObject>();
    //public MonsterStageList stage;

    public GameObject returnTown;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var m in monsterSpawn)
            {
                GameObject go = PhotonNetwork.Instantiate(m.monster.name, m.transform.position, Quaternion.identity);
                //monsters.Add(go);
            }
        }
        //GameManager.instance.enemyList = monsterList;
    }

    private void OnEnable()
    {
        photonView.RPC(nameof(SetStageRPC), RpcTarget.All);

        //int size = monsterList.childCount;

        //for(int i = 0; i < size; i++)
        //{
        //    monsters[i].transform.position = monsterSpawn[i].transform.position;
        //    monsters[i].SetActive(true);
        //    //monsterList.GetChild(i).gameObject.SetActive(true);
        //}
    }

    [PunRPC]
   private void SetStageRPC()
   {
        int size = monsterList.childCount;

        for (int i = 0; i < size; i++)
        {
            monsters[i].transform.position = monsterSpawn[i].transform.position;
            monsters[i].SetActive(true);
            //monsterList.GetChild(i).gameObject.SetActive(true);
        }
   }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GameManager.instance.stage.Enqueue(this);
        GameManager.instance.SetNextStage();
        gameObject.SetActive(false);
    }
}