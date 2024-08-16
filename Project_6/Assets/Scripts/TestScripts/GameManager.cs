using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    public Queue<StageData> stage { get; private set; } /*= new Queue<StageData>() {}*/

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
        stage = new Queue<StageData>();
        cam = Camera.main.GetComponent<CameraController>();

        if(PhotonNetwork.IsMasterClient)
        {
            foreach(var m in maps)
            {
                //GameObject go = Instantiate(m, new Vector2(100, 0), Quaternion.identity);
                GameObject go = PhotonNetwork.Instantiate(m.name, new Vector2(100, 0), Quaternion.identity);
                //stage.Enqueue(go.GetComponent<StageData>());
                //go.SetActive(false);
            }
        }
        //SetNextStage();
    }

    public Transform SpawnStage()
    {
        return enemyList;
    }

    public void SetNextStage()
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

    public void PlayerDie()
    {
        dieCount++;

        if (dieCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            if (BossBattleManager.instance.spawnedBoss != null)
            {
                PhotonNetwork.Destroy(BossBattleManager.instance.spawnedBoss);
                stage.Peek().monsterList.gameObject.SetActive(true);
            }

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
