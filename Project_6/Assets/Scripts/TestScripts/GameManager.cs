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

    public GameObject[] maps; //�������ϴ� �ʵ�

    //�����ϰ������=================
    public int cleaStageCount;
    //public Transform[] enemyList; //������ ������ġ
    //public DestinationData[] nextStage; //���� ��������

    Transform enemyList; //������ ������ġ
    //DestinationData nextStage; //���� ��������

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
        //    GameObject go = Instantiate(m, new Vector2(100, 0), Quaternion.identity); //�ʻ���

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
