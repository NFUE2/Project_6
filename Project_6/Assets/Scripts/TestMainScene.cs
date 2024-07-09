using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class TestMainScene : PunSingleton<TestMainScene>//, IPunObservable
{
    //캐릭터 생성
    //Dictionary<int, GameObject> dict = new Dictionary<int, GameObject>();
    //[SerializeField] List<GameObject> list = new List<GameObject>();
    //int[] nums = new int[5];
    //public GameObject character;

    Dictionary<int, int> dict = new Dictionary<int, int>(); //리스트는 포톤에서 동기화 안됨
    public TextMeshProUGUI playerIDList;

    private void Update()
    {
        //Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.Log(dict.Count);
    }

    public override void OnJoinedRoom() //방 난입에도 작동함
    {
        base.OnJoinedRoom();
        //Debug.Log(dict.Count);
    }

    public void CreateRPC(int id)
    {
        //character = go;
        photonView.RPC(nameof(CreateCharacter),RpcTarget.AllBuffered,id, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    public void CreateCharacter(int actor,int id) //현재 플레이어에게 오브젝트 번호 연동
    {
        //GameObject createGo = PhotonNetwork.Instantiate(character.name,Vector3.zero,Quaternion.identity);

        //Debug.Log(dict.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber));
        //if (dict.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber))
        //{
        //    Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        //    Debug.Log("X");
        //    return;
        //}

        //dict.Add(PhotonNetwork.LocalPlayer.ActorNumber, character);
        //    Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        //dict.Add(dict.Count, character);
        //Debug.Log(dict.Count);

        //list.Add(character);
        //Debug.Log(list.Count);

        dict.Add(id,actor);
        //playerIDList.text += $"\n{actor} {id}";
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(dict);
    //        //stream.SendNext(list);
    //        //stream.SendNext(nums);
    //    }
    //    else
    //    {
    //        dict = (Dictionary<int, int>)stream.ReceiveNext();
    //        //dict = (Dictionary<int, GameObject>)stream.ReceiveNext();
    //        //list = (List<GameObject>)stream.ReceiveNext();
    //        //nums = (int[])stream.ReceiveNext();
    //    }
    //}
}
