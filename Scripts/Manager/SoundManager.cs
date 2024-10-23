using UnityEngine;

public enum BGMList
{
    Intro,
    Town,
    Stage1,
    Stage1Boss,
    Stage2,
    Stage2Boss,
}

public enum SourceType
{
    BGM,
    SFX,
}

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource[] audios;
    public AudioClip[] clipBGM;

    public void ChangeBGM(BGMList b)
    {
        audios[(int)SourceType.BGM].clip = clipBGM[(int)b];
        audios[(int)SourceType.BGM].Play();
    }

    public void Shot(AudioClip clip)
    {
        audios[(int)SourceType.SFX].PlayOneShot(clip);
    }

    public void SetVolume(SourceType type,float value)
    {
        audios[(int)type].volume = value;
    }

    //private void SetVolume()
    //{
    //    for(int i = 0; i < sources.Length; i++)
    //        sources[i].volume = DataManager.instance.data.volums[i];
    //}
}