using UnityEngine;
using UnityEngine.Audio;

public enum BGMList
{
    Intro,
    Town,
    Stage1,
    Stage1Boss,
    Stage2,
    Stage2Boss,
}

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource BGM,EFF;
    public AudioClip[] clipBGM;

    public override void Awake()
    {
        base.Awake();
        ChangeBGM(BGMList.Intro);
    }

    public void ChangeBGM(BGMList b)
    {
        BGM.clip = clipBGM[(int)b];
        BGM.Play();
    }

    public void Shot(AudioClip clip)
    {
        EFF.PlayOneShot(clip);
    }
}
