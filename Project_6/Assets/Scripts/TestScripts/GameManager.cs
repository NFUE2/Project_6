using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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

    int dieCount;

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
    }

    public void StageClear()
    {
        cleaStageCount++;

        if (cleaStageCount == maps.Length) SceneControl.instance.LoadScene(SceneType.Outro);
        StageData data = stage.Peek();
        data.returnTown.SetActive(true);

        stage.Dequeue();
        SetNextStage();
    }

    public void PlayerDie()
    {
        dieCount++;

        if(dieCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            dieCount = 0;

            stage.Peek().gameObject.SetActive(false);

            player.transform.position = town.startTransform.position;
            cam.target = town.CameraPos;
            SoundManager.instance.ChangeBGM(BGMList.Town);

            if (player.TryGetComponent(out PlayerCondition p))
                p.Resurrection();
        }
    }
}
