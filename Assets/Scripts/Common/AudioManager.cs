using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    //array index = scene index
    [SerializeField] AudioSource[] backgroundAudio;
    private int currentBackgroundIndex = -1;

    public void ChangeBackgroundAudio(int newAudioIndex)
    {
        if (newAudioIndex > backgroundAudio.Length) return;
        if (currentBackgroundIndex == -1)
        {
            backgroundAudio[newAudioIndex].Play();
        }
        else
        {
            if (backgroundAudio[currentBackgroundIndex].isPlaying) backgroundAudio[currentBackgroundIndex].Stop();
            backgroundAudio[newAudioIndex].Play();
        }
        currentBackgroundIndex = newAudioIndex;
    }
    public void StopCurrentTrack()
    {
        if (currentBackgroundIndex != -1 && backgroundAudio[currentBackgroundIndex].isPlaying) backgroundAudio[currentBackgroundIndex].Stop();
        currentBackgroundIndex = -1;
    }
}
