using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    //array index = scene index
    [SerializeField] AudioSource[] backgroundAudio;
    private int currentBackgroundIndex = 0;
    // 0 Basic - 1 Ability One - 2 Ability Two - 3 Ability Three
    // 4 Lightning
    [SerializeField] AudioSource[] ArtemisAudio;
    // 4 Trap Trigger
    [SerializeField] AudioSource[] JackAudio;
    [SerializeField] AudioSource[] DeimosAudio;

    public void PlayArtemisAudio(int audioIndex)
    {
        ArtemisAudio[audioIndex].Play();
    }
    public void PlayJackAudio(int audioIndex)
    {
        JackAudio[audioIndex].Play();
    }
    public void PlayDeimosAudio(int audioIndex)
    {
        DeimosAudio[audioIndex].Play();
    }

    public void ChangeBackgroundAudio(int newAudioIndex)
    {
        backgroundAudio[currentBackgroundIndex].Stop();
        backgroundAudio[newAudioIndex].Play();
        currentBackgroundIndex = newAudioIndex;
    }
}
