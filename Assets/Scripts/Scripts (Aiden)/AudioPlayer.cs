using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlayer : MonoBehaviour
{
    private string[] availibleAudioTypes = new string[] { ".mp3", ".wav" };

    public List<AudioSource> audioSource;
    bool loop = false;
    public static GameObject player;
    public List<GameObject> audioObject;

    public AudioPlayer()
    {

    }

    private void Update()
    {
        if ()
        {

        }
    }

    public void Play(int index = -1)
    {
        if (index >= 0)
        {
            if (index < audioSource.Count)
            {
                if (audioSource[index].clip != null)
                {
                    audioSource[index].Play();
                }
            }
        }
        else
        {
            foreach (AudioSource audioSource in audioSource)
            {
                audioSource.Play();
            }
        }
    }
    public void Stop(int index = -1)
    {
        if (index >= 0)
        {
            if (index < audioSource.Count)
            {
                audioSource[index].Stop();
            }
        }
        else
        {
            foreach (AudioSource audioSource in audioSource)
            {
                audioSource.Stop();
            }
        }
    }

    public void Pause(int index = -1)
    {
        if (index >= 0)
        {
            if (index < audioSource.Count)
            {
                audioSource[index].Pause();
            }
        }
        else
        {
            foreach (AudioSource audioSource in audioSource)
            {
                audioSource.Pause();
            }
        }     
    }

    public void Resume(int index = -1)
    {
        if (index >= 0)
        {
            if (index < audioSource.Count)
            {
                audioSource[index].UnPause();
            }
        }
        else
        {
            foreach (AudioSource audioSource in audioSource)
            {
                audioSource.UnPause();
            }
        }
    }

    private List<string[]> GetAudioFiles(string path)
    {
        List<string[]> files = new();
        foreach (var audioType in availibleAudioTypes)
        {
            files.Add(Directory.GetFiles(path, "*" + audioType));
        }
        return files;
    }

    IEnumerator LoadAudioClip(string filePath, string audioType, System.Action<AudioClip> callback)
    {
        UnityWebRequest www = new();
        switch (audioType)
        {
            case ".wav":
                www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV);
                break;
            case ".mp3":
                www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG);
                break;
            default:
                break;
        }

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            callback?.Invoke(clip);
        }
        else
        {
            callback?.Invoke(null);
        }
    }
}
