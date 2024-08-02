using Photon.Pun.Demo.Cockpit;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    Queue<StageData> stage = new Queue<StageData>();

    Transform enemyList; //적들의 생성위치
    //DestinationData nextStage; //다음 스테이지
    [field:SerializeField] public DestinationData town { get; private set; }

    public VotingObject voting;
    public GameObject vote;
    //================================

    public override void Awake()
    {
        base.Awake();
        cleaStageCount = 0;
        cam = Camera.main.GetComponent<CameraController>();

        foreach(var m in maps)
        {
            GameObject go = Instantiate(m, new Vector2(100, 0), Quaternion.identity);
            stage.Enqueue(go.GetComponent<StageData>());
            go.SetActive(false);
        }

        SetNextStage();
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
        //SetNextStage();
    }

    public Transform SpawnStage()
    {
        return enemyList;
    }

    void SetNextStage()
    {
        if (stage.Count == 0) return;

        StageData data = stage.Peek();
        enemyList = data.monsterList;
        voting.data = data.data;

        //voting.data = curMap.data;
        //GameObject go = Instantiate(maps[cleaStageCount], new Vector2(100, 0), Quaternion.identity); ;

        //curMap = go.GetComponent<StageData>();
        //enemyList = curMap.monsterList;
        //go.SetActive(false);
    }

    public void StageClear()
    {
        cleaStageCount++;
        StageData data = stage.Peek();
        data.returnTown.SetActive(true);

        stage.Dequeue();
        SetNextStage();
    }

    //public void StageClear()
    //{
    //    cleaStageCount++;
    //    Destroy(curMap);
    //    SetNextStage();
    //}
}
