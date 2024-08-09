using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControllerSetting : MonoBehaviour
{
    private string type;
    private void Start()
    {
        type = gameObject.GetComponent<VolumeType>().GetVolumeType();
        SettingSlider(type);
    }
    private void SettingSlider(string audioSourceType)
    {
        var slider = gameObject.GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = SoundManager.Instance.GetAudioSource(audioSourceType).volume;

        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, audioSourceType));
    }

    private void OnSliderValueChanged(float value, string audioSourceType)
    {
        AudioSource audioSource = SoundManager.Instance.GetAudioSource(audioSourceType);
        if (audioSource != null)
        {
            audioSource.volume = value;
        }
    }
}
