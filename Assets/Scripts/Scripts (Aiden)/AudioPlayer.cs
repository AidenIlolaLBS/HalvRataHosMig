using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    public bool multiSource = false;
    public bool loop = false;
    public string audioPlayerName;

    public AudioPlayer(AudioSource audioSource, bool multiSource, bool loop, string audioPlayerName, AudioClip audioClip = null)
    {
        this.audioSource = audioSource;
        this.clip = audioClip;
        this.multiSource = multiSource;
        this.loop = loop;
        this.audioPlayerName = audioPlayerName;
    }

    public void StartClip()
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopClip()
    {
        audioSource.Stop();
    }
}
