using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Threading.Tasks;
using System.Collections;
using UnityEditor.AddressableAssets.GUI;

public class LoadingScene : MonoBehaviourPun
{
    public string[] message;
    public TextMeshProUGUI text;
    public Slider slider;

    public DataManager dataManager;

    bool isLoadData;

    private void Start()
    {
        int i = Random.Range(0, message.Length);
        text.text = message[i];

        isLoadData = false;
        //Loading();
        StartCoroutine(Loading());
    }

    //private async void Loading()
    //{
    //    //bool isDone = await dataManager.DataLoad();
    //    var oper = PhotonNetwork.LoadLevelAsync((int)Scene.Main);

    //    while(!oper.isDone)
    //    {

    //    }
    //}

    private IEnumerator Loading()
    {
        Debug.Log("로딩시작");
        var oper = PhotonNetwork.LoadLevelAsync((int)Scene.Main);

        oper.allowSceneActivation = false;
        LoadAsset();

        //yield return null;

        while (!oper.allowSceneActivation /*!oper.isDone && !isDataLoad*/)
        {
            yield return null;

            float progress = PhotonNetwork.LevelLoadingProgress;

            if(isLoadData && slider.value >= 0.9f) oper.allowSceneActivation = true;
            else
                slider.value = Mathf.MoveTowards(slider.value, progress, Time.deltaTime);
        }

        SoundManager.instance.ChangeBGM(BGMList.Town);
    }

    private async void LoadAsset()
    {
        await dataManager.DataLoad();
        isLoadData = true;
    }
}
