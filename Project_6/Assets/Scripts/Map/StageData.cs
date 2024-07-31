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
    public DestinationData data;

    public MonsterStageList stage;

    private void Awake()
    {
        GameManager.instance.nextStage[(int)stage] = data;
    }

    private void Start()
    {
        foreach (var m in monsterSpawn)
            PhotonNetwork.Instantiate($"Monster/{stage.ToString()}/{m.monster.name}", m.transform.position, Quaternion.identity);

        GameManager.instance.enemyList[(int)stage] = monsterList;
    }
}