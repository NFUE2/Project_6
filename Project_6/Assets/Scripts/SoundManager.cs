using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : Singleton<SoundManager>
{
    public AudioSource BGM,EFF;

    public void Shot(AudioClip clip)
    {
        EFF.PlayOneShot(clip);
    }

}
