using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAudioOnOpen : MonoBehaviour
{
    [SerializeField] int newAudioIndex = 0;
    void Start()
    {
        AudioManager.Instance.ChangeBackgroundAudio(newAudioIndex);
    }
}
