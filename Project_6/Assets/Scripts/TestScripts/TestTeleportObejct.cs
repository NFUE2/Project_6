using System;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class StageData
{
    public GameObject stage,stageBackground;
    public Transform destination;
    public BGMList bgm;
}

public class TestTeleportObejct : MonoBehaviour//, TestIInteraction
{
    //public Transform destination;
    public StageData data;

    //public void OnInteraction()
    //{
    //    //destination.position;
    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        data.stage.SetActive(!data.stage.activeInHierarchy);
        data.stageBackground.SetActive(!data.stageBackground.activeInHierarchy);

        collision.transform.position = data.destination.position;
        //collision.transform.position = destination.position;
        TestGameManager.instance.cam.target = TestGameManager.instance.player.transform;
        SoundManager.instance.ChangeBGM(data.bgm);
    }
}
