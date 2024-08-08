using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections;

public class Loading : MonoBehaviourPun
{
    public string[] message;
    public TextMeshProUGUI text;
    public Slider slider;

    public DataManager dataManager;
    public GameObject loadingPanel;

    int isReady = 0;
    bool isLoad = false;

    private void Start()
    {
        int i = Random.Range(0, message.Length);
        text.text = message[i];
        //Loading();
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        LoadAsset();

        while (true /*!oper.isDone && !isDataLoad*/)
        {
            yield return null;

            if (!isLoad)
                slider.value = Mathf.MoveTowards(slider.value, 0.5f, Time.deltaTime);
            else
                slider.value = Mathf.MoveTowards(slider.value, 1f, Time.deltaTime);

            if (slider.value >= 1.0f && AllReady() && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel((int)SceneType.Main);
                break;
            }
        }

        //loadingPanel.SetActive(false);
    }

    private async void LoadAsset()
    {
        isLoad = await dataManager.DataLoad();
        photonView.RPC(nameof(IsReady), RpcTarget.All);
    }


    [PunRPC] private void IsReady() { isReady++; } 
    private bool AllReady()  { return isReady == PhotonNetwork.CurrentRoom.PlayerCount; }
}
