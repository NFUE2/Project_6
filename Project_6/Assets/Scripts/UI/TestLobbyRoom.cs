using UnityEngine;
using Photon.Realtime;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
using System.Text;

public class TestLobbyRoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI currentPlayer;
    public TestLobbyManager lobbyManager;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        roomName.text = roomInfo.Name;
        //StringBuilder player = new StringBuilder($"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}");
        currentPlayer.text = $"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";
    }

    public void OnClick()
    {
        lobbyManager.choiceRoomName = roomName.text;
    }
    

}
