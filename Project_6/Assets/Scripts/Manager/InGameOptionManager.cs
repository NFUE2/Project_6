using UnityEngine;
using UnityEngine.UI;

public class InGameOptionManager : MonoBehaviour
{
    public GameObject optionUI;
    public Slider BGMSlider;
    public Slider EFFSlider;

    private void Start()
    {
        BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        EFFSlider.onValueChanged.AddListener(SetEFFVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionUI.SetActive(!optionUI.activeSelf);
        }
    }

    public void SetBGMVolume(float value)
    {
        SoundManager.Instance.audios[(int)SourceType.BGM].volume = value;
    }

    public void SetEFFVolume(float value)
    {
        SoundManager.Instance.audios[(int)SourceType.SFX].volume = value;
    }
}