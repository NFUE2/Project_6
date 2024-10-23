using Photon.Pun;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class OptionUI : MonoBehaviour
{
    public Slider[] sliders;
    public void OnClickLeaveGame()
    {
        PhotonNetwork.Disconnect();
        //PhotonNetwork.LoadLevel(0);
        SceneControl.instance.LoadScene(SceneType.Intro);
        SoundManager.instance.ChangeBGM(BGMList.Intro);
    }

    private void OnEnable()
    {
        for(int i = 0; i < sliders.Length; i++)
            sliders[i].value = DataManager.instance.saveData.volums[i];
    }

    private void OnDisable()
    {
        var audios = SoundManager.instance.audios;

        for (int i = 0; i < audios.Length; i++)
            DataManager.instance.saveData.volums[i] = audios[i].volume;

        DataManager.instance.Save();
    }
}
