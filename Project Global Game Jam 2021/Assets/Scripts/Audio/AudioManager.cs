using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup master;

    private void Awake() 
    {
        AudioSource[] allAudioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        for (int i = 0; i < allAudioSources.Length; i++)
        {
            allAudioSources[i].outputAudioMixerGroup = master;
        }
    }

}
