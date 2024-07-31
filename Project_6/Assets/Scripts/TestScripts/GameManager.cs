using System.Collections.Generic;
using UnityEngine;

public enum MonsterStageList
{
    Stage1,
    Stage2,
    Stage3,

}

public class GameManager : Singleton<GameManager>
{
    public CameraController cam { get; private set; }
    public GameObject player;
    public List<GameObject> players = new List<GameObject>();
    public Transform[] enemyList;

    public override void Awake()
    {
        base.Awake();
        cam = Camera.main.GetComponent<CameraController>();
    }

    public Transform SpawnStage(MonsterStageList stage)
    {
        return enemyList[(int)stage];
    }
}
