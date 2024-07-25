using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using Photon.Pun;
using System.Data;

[RequireComponent(typeof(PhotonView))]
public class UISlot : MonoBehaviourPun //, IPunObservable
{
    public GameObject prefab;
    public Transform spawnPoint;
    public ObjectSO character;

    public SkillDataSO Skill_Q;
    public SkillDataSO Skill_E;


    public ChooseJob_Board02 Board_02;

    private TextMeshProUGUI characterName;
    private Image image;
    CameraController camController;
    Button selectButton;

    private void Start()
    {
        characterName = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
        selectButton = GetComponent<Button>();

        //characterName.text = data.name;
        //image.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
    }

    public void OnClick(/*GameObject panel*/)
    {
        Board_02.ChooseJob(character,Skill_Q,Skill_E);
        Board_02.ChooseJob(prefab, this);

        //Board_02.choicePlayer = prefab;

        //panel.SetActive(false);
        //TestMainScene.instance.CreateRPC(prefab);
        //TestMainScene.instance.CreateRPC(data.id);
        //TestGameManager.instance.player = PhotonNetwork.Instantiate(prefab.name, spawnPoint.position, Quaternion.identity); //�ش� ������Ʈ Photon View�ʿ�
        //photonView.RPC(nameof(OnClickRPC),RpcTarget.AllBuffered, TestGameManager.instance.player);
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TestCameraController>().target = go.transform;
        //TestGameManager.instance.cam.target = TestGameManager.instance.player.transform;
    }

    public void ChangeInteraction()
    {
        photonView.RPC(nameof(OnClickRPC),RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void OnClickRPC()
    {
        selectButton.interactable = false;
    }

    //[PunRPC]
    //private void OnClickRPC(GameObject player)
    //{
    //    selectButton.interactable = false;
    //    TestGameManager.instance.players.Add(player);
    //}

    //AllBuffered�� �� �ʿ� ����
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if(stream.IsWriting)
    //    {
    //        stream.SendNext(selectButton.interactable);
    //    }
    //    else
    //    {
    //        selectButton.interactable = (bool)stream.ReceiveNext();
    //    }
    //}
}
