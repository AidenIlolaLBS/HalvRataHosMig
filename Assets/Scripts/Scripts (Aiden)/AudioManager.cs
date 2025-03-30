using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public enum SoundType
{
    MenuMusic,
    GameMusic,
    ButtonSound,
    WalkingSound,
    ChopSound,
    CauldronSound,
    DoorSound,    
    FridgeSound,
    PickUpSound,
    DropSound,
    ThrowSound,
    DialogueSound,
    Ambiance
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundList[] SoundList;

    public bool inGame = false;

    [SerializeField, Range(0, 1)]
    double masterVolume;
    public double MasterVolume
    {
        get { return masterVolume; }
    }

    [SerializeField, Range(0, 1)]
    double musicVolume;
    public double MusicVolume
    {
        get { return musicVolume; }
    }
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    bool shouldPlayMusic = true;

    [SerializeField, Range(0, 1)]
    double sfxVolume;
    public double SfxVolume
    {
        get { return sfxVolume; }
    }
    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField, Range(0, 1)]
    double ambianceVolume;
    public double AmbianceVolume
    {
        get { return ambianceVolume; }
    }
    [SerializeField]
    private AudioSource ambianceSource;

    [SerializeField, Range(0, 1)]
    double dialogueVolume;
    public double DialogueVolume
    {
        get { return dialogueVolume; }
    }
    [SerializeField]
    private AudioSource dialogueSource;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        UpdateAllVolumeValues(masterVolume, musicVolume, sfxVolume, ambianceVolume, dialogueVolume);
    }

    private void Update()
    {
        
    }

    public void UpdateAllVolumeValues(double _masterVolume, double _musicVolume, double _sfxVolume, double _ambianceVolume, double _dialogueVolume)
    {
        masterVolume = _masterVolume;
        musicVolume = _masterVolume * _musicVolume;
        sfxVolume = _masterVolume * _sfxVolume;
        ambianceVolume = _masterVolume * _ambianceVolume;
        dialogueVolume = _masterVolume * _dialogueVolume;
    }

    public void StartMusic(string musicType)
    {
        StopMusic();
        shouldPlayMusic = true;
        System.Random rnd = new();
        List<string[]> audioFiles = GetAudioFiles(Application.streamingAssetsPath + "/Audio" + "/MusicAudio" + "/" + musicType);
        int typeOfAudio = rnd.Next(0, availibleAudioTypes.Length);
        StartCoroutine(LoadAudioClip(audioFiles[typeOfAudio][rnd.Next(0, audioFiles[typeOfAudio].Length)], availibleAudioTypes[typeOfAudio], (clip) =>
        {
            musicSource.clip = clip;
            musicSource.Play();
        }));
    }
    public void StopMusic()
    {
        Destroy(musicSource);
        musicSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartSFX(string sfxType, bool onlyOne = true, GameObject player = null)
    {
        System.Random rnd = new();
        List<string[]> audioFiles = GetAudioFiles(Application.streamingAssetsPath + "/Audio" + "/SFXAudio" + "/" + sfxType);
        int typeOfAudio = rnd.Next(0, availibleAudioTypes.Length);
        StartCoroutine(LoadAudioClip(audioFiles[typeOfAudio][rnd.Next(0, audioFiles[typeOfAudio].Length)], availibleAudioTypes[typeOfAudio], (clip) =>
        {
            sfxSoloSource.clip = clip;
            sfxSoloSource.Play();
        }));
    }

    public void StartDialogue(string clipPath)
    {
        if (clipPath == "")
        {
            return;
        }
        StopDialogue();
        System.Random rnd = new();
        List<string[]> audioFiles = GetAudioFiles(clipPath);
        int typeOfAudio = rnd.Next(0, availibleAudioTypes.Length);
        StartCoroutine(LoadAudioClip(audioFiles[typeOfAudio][rnd.Next(0, audioFiles[typeOfAudio].Length)], availibleAudioTypes[typeOfAudio], (clip) =>
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
        List<string[]> audioFiles = GetAudioFiles(Application.streamingAssetsPath + "/Audio" + "/AmbianceAudio");
        int typeOfAudio = rnd.Next(0, availibleAudioTypes.Length);
        StartCoroutine(LoadAudioClip(audioFiles[typeOfAudio][rnd.Next(0, audioFiles[typeOfAudio].Length)], availibleAudioTypes[typeOfAudio], (clip) =>
        {
            ambianceSoloSource.clip = clip;
            ambianceSoloSource.Play();
        }));
    }
    public void StopAmbiance()
    {
        ambianceSoloSource.Stop();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref SoundList, names.Length);
        for (int i = 0; i < SoundList.Length; i++)
        {
            SoundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct  SoundList
{
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
