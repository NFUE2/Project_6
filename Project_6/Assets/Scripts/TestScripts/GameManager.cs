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

    public GameObject[] maps; //만들어야하는 맵들

    //수정하고싶은곳=================
    public int cleaStageCount;
    //public Transform[] enemyList; //적들의 생성위치
    //public DestinationData[] nextStage; //다음 스테이지

    Transform enemyList; //적들의 생성위치
    //DestinationData nextStage; //다음 스테이지

    [field:SerializeField] public DestinationData town { get; private set; }

    public VotingObject voting;
    public GameObject vote;
    //================================

    public override void Awake()
    {
        base.Awake();
        cam = Camera.main.GetComponent<CameraController>();
        //enemyList = new Transform[maps.Length];
        //nextStage = new DestinationData[maps.Length];

        //foreach (var m in maps)
        //{
        //    GameObject go = Instantiate(m, new Vector2(100, 0), Quaternion.identity); //맵생성

        //    StageData stage = go.GetComponent<StageData>();
        //    enemyList[(int)stage.stage] = stage.monsterList;
        //    nextStage[(int)stage.stage] = stage.data;

        //    go.SetActive(false);
        //}
        SetNextStage();
    }

    public Transform SpawnStage()
    {
        return enemyList;
    }

    public void SetNextStage()
    {
        GameObject go = Instantiate(maps[cleaStageCount],new Vector2(100,0),Quaternion.identity);

        StageData stage = go.GetComponent<StageData>();
        enemyList = stage.monsterList;
        voting.data =/* nextStage = */stage.data;
        go.SetActive(false);
    }
}
