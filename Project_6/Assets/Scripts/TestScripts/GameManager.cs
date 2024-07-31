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

    public GameObject[] maps;

    public override void Awake()
    {
        base.Awake();
        cam = Camera.main.GetComponent<CameraController>();
        enemyList = new Transform[maps.Length];

        foreach (var m in maps)
        {
            GameObject go = Instantiate(m, new Vector2(100, 0), Quaternion.identity);
            StageData stage = go.GetComponent<StageData>();
            enemyList[(int)stage.stage] = stage.monsterList;
            go.SetActive(false);
        }
    }

    public Transform SpawnStage(MonsterStageList stage)
    {
        return enemyList[(int)stage];
    }
}
