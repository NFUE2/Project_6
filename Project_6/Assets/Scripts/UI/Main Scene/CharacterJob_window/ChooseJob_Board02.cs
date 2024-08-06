using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ChooseJob_Board02 : MonoBehaviourPun
{

    public TextMeshProUGUI Character_Name;
    public TextMeshProUGUI Character_Info;
    public Image Skill_Q;
    public Image Skill_E;
    public TextMeshProUGUI Q_Info;
    public TextMeshProUGUI E_Info;
    public Button Button;

    private GameObject choicePlayer; //추후 수정
    public GameObject panel;
    public UISlot choicebButton;

    public void ChooseJob(ObjectSO objectSO, SkillDataSO Skill_Q, SkillDataSO Skill_E)
    {
        Character_Name.text = objectSO.name;
        Character_Info.text = objectSO.info;
        Q_Info.text = Skill_Q.info;
        E_Info.text = Skill_E.info;
        

        //Skill_Q = ;
        //Skill_E = ;
    }
    public void ChooseJob(GameObject player, UISlot button)
    {
        choicePlayer = player;
        choicebButton = button;
    }

    public void OnClick()
    {
        if (choicebButton == null || !choicebButton.GetComponent<Button>().interactable) return;
        GameObject go = PhotonNetwork.Instantiate(choicePlayer.name,Vector2.zero,Quaternion.identity);
        //GameManager.instance.player = go;
        //GameManager.instance.players.Add(go);

        //photonView.RPC(nameof(OnClickRPC), RpcTarget.AllBuffered, choicebButton);
        choicebButton.ChangeInteraction();

        panel.SetActive(false);
    }

    //추후 스킬 수정되면가져오기
    //public void Display(PlayerBase player)
    //{
    //    Character_Name.text = player.playerData.name;
    //    Character_Info.text = player.playerData.info;

    //}

    //[PunRPC]
    //public void OnClickRPC(Button button)
    //{
    //    Debug.Log(button);
    //    button.interactable = false;
    //    //photonView.instan
    //}
}
