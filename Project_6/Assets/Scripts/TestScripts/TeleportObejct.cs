using Photon.Pun;
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

[RequireComponent(typeof(PhotonView))]
public class TeleportObejct : MonoBehaviourPun//, TestIInteraction
{
    //public Transform destination;
    public StageData data;

    //public void OnInteraction()
    //{
    //    //destination.position;
    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //data.stage.SetActive(!data.stage.activeInHierarchy);
        if (collision.gameObject == GameManager.instance.player)
            data.stageBackground.SetActive(!data.stageBackground.activeInHierarchy);

        collision.transform.position = data.destination.position;
        //collision.transform.position = destination.position;
        GameManager.instance.cam.target = GameManager.instance.player.transform;
        SoundManager.instance.ChangeBGM(data.bgm);
    }
}
