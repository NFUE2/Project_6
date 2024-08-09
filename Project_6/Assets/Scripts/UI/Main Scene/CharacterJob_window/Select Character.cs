using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class SelectCharacter : MonoBehaviourPun
{
    public GameObject prefab;
    public Transform spawnPoint;

    Button selectButton;

    public void SelectJob(GameObject panel)
    {
        panel.SetActive(false);
        //TestMainScene.instance.CreateRPC(prefab);
        //TestMainScene.instance.CreateRPC(data.id);
        GameManager.instance.player = PhotonNetwork.Instantiate(prefab.name, spawnPoint.position, Quaternion.identity); //해당 오브젝트 Photon View필요
        //photonView.RPC(nameof(OnClickRPC), RpcTarget.AllBuffered, GameManager.instance.player);
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TestCameraController>().target = go.transform;
        GameManager.instance.cam.target = GameManager.instance.player.transform;
    }

    //[PunRPC]
    //private void OnClickRPC(GameObject player)
    //{
    //    selectButton.interactable = false;
    //    GameManager.instance.players.Add(player);
    //}

}