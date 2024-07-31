using Photon.Pun;
using System;
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

    public MonsterStageList stage;
    
    private void Start()
    {
        foreach (var m in monsterSpawn)
            PhotonNetwork.Instantiate("Monster/" + m.monster.name, m.transform.position, Quaternion.identity);

        GameManager.instance.enemyList[(int)stage] = monsterList;
    }
}
