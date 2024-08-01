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

public class StageData : MonoBehaviour
{
    public MonsterSpawn[] monsterSpawn;
    public Transform monsterList;
    public DestinationData data;
    public List<GameObject> monsters = new List<GameObject>();
    //public MonsterStageList stage;

    public bool isClear;

    //private void Awake()
    //{
    //    //GameManager.instance.nextStage[(int)stage] = data;
    //}

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var m in monsterSpawn)
            {
                GameObject go = PhotonNetwork.Instantiate($"Monster/{m.monster.name}", m.transform.position, Quaternion.identity);
                monsters.Add(go);
            }
        }

        isClear = false;
        //GameManager.instance.enemyList = monsterList;
    }

    private void OnEnable()
    {
        int size = monsterList.childCount;

        for(int i = 0; i < size; i++)
        {
            monsters[i].transform.position = monsterSpawn[i].transform.position;
            monsters[i].SetActive(true);
            //monsterList.GetChild(i).gameObject.SetActive(true);
        }
    }
}