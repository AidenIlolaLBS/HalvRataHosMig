using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    bool inGame = false;

    [SerializeField, Range(0, 1)]
    double masterVolume;
    public double MasterVolume
    {
        get { return masterVolume; }
    }

    [SerializeField]
    AudioSource musicSource;
    [SerializeField, Range(0, 1)]
    double musicVolume;
    public double MusicVolume
    {
        get { return musicVolume; }
    }
    [SerializeField]
    bool shouldPlayMusic = true;

    [SerializeField]
    AudioSource sfxSource;
    [SerializeField, Range(0, 1)]
    double sfxVolume;
    public double SfxVolume
    {
        get { return sfxVolume; }
    }

    [SerializeField]
    AudioSource ambianceSource;
    [SerializeField, Range(0, 1)]
    double ambianceVolume;
    public double AmbianceVolume
    {
        get { return ambianceVolume; }
    }

    [SerializeField]
    AudioSource dialogueSource;
    [SerializeField, Range(0, 1)]
    double dialogueVolume;
    public double DialogueVolume
    {
        get { return dialogueVolume; }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        musicSource.loop = false;
        sfxSource.loop = false;
        dialogueSource.loop = false;
        ambianceSource.loop = false;
        UpdateAllVolumeValues(masterVolume, musicVolume, sfxVolume, ambianceVolume, dialogueVolume);
        StartMusic("MainMenu");
    }
    private void Update()
    {
        if (!musicSource.isPlaying && shouldPlayMusic)
        {
            if (inGame)
            {
                StartMusic("Game");
            }
            else
            {
                StartMusic("MainMenu");
            }
        }
    }

    public void UpdateAllVolumeValues(double _masterVolume, double _musicVolume, double _sfxVolume, double _ambianceVolume, double _dialogueVolume)
    {
        masterVolume = _masterVolume;
        musicVolume = _masterVolume * _musicVolume;
        sfxVolume = _masterVolume * _sfxVolume;
        ambianceVolume = _masterVolume * _ambianceVolume;
        dialogueVolume = _masterVolume * _dialogueVolume;
        ChangeVolume();
    }

    private void ChangeVolume()
    {
        musicSource.volume = (float)musicVolume;
        ambianceSource.volume = (float)ambianceVolume;
        dialogueSource.volume = (float)dialogueVolume;
        sfxSource.volume = (float)sfxVolume;
    }

    public void StartMusic(string musicType)
    {
        StopMusic();
        shouldPlayMusic = true;
        System.Random rnd = new();
        string[] audioFiles = Directory.GetFiles(Application.streamingAssetsPath + "/Audio" + "/MusicAudio" + "/" + musicType, "*.wav");
        StartCoroutine(LoadAudioClip(audioFiles[rnd.Next(0, audioFiles.Length)], (clip) =>
        {
            musicSource.clip = clip;
            musicSource.Play();
        }));
    }
    public void StopMusic()
    {
        shouldPlayMusic = false;
        musicSource.Stop();
    }

    public void StartSFX(string sfxType)
    {
        System.Random rnd = new();
        string[] audioFiles = Directory.GetFiles(Application.streamingAssetsPath + "/Audio" + "/SFXAudio" + "/" + sfxType, "*.wav");
        StartCoroutine(LoadAudioClip(audioFiles[rnd.Next(0, audioFiles.Length - 1)], (clip) =>
        {
            sfxSource.clip = clip;
            sfxSource.Play();
        }));
    }
    private void StopSFX()
    {
        sfxSource.Stop();
    }

    public void StartDialogue(string clipPath)
    {
        if (clipPath == "")
        {
            return;
        }
        StopDialogue();
        StartCoroutine(LoadAudioClip(clipPath, (clip) =>
        {
            dialogueSource.clip = clip;
            dialogueSource.Play();
        }));
    }
    public void StopDialogue()
    {
        dialogueSource.Stop();
    }

    public void StartAmbiance()
    {
        StopAmbiance();
        System.Random rnd = new();
        string[] audioFiles = Directory.GetFiles(Application.streamingAssetsPath + "/Audio" + "/AmbianceAudio", "*.wav");
        StartCoroutine(LoadAudioClip(audioFiles[rnd.Next(0, audioFiles.Length)], (clip) =>
        {
            ambianceSource.clip = clip;
            ambianceSource.Play();
        }));
    }
    public void StopAmbiance()
    {
        ambianceSource.Stop();
    }

    IEnumerator LoadAudioClip(string filePath, System.Action<AudioClip> callback)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV);
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
