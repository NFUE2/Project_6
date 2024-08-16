using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    public Queue<StageData> stage { get; private set; } /*= new Queue<StageData>() {}*/

    Transform enemyList; //������ ������ġ
    //DestinationData nextStage; //���� ��������
    [field:SerializeField] public DestinationData town { get; private set; }

    public VotingObject voting;
    public GameObject vote;
    //================================

    public override void Awake()
    {
        base.Awake();
        cleaStageCount = 0;
        stage = newQueue<StageData>();
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
        Debug.Log(stage.Count);
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
}
