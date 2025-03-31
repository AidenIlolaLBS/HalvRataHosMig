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
    float masterVolume;
    public float MasterVolume
    {
        get { return masterVolume; }
    }

    [SerializeField, Range(0, 1)]
    float musicVolume;
    public float MusicVolume
    {
        get { return musicVolume; }
    }
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    bool shouldPlayMusic = true;

    [SerializeField, Range(0, 1)]
    float sfxVolume;
    public float SfxVolume
    {
        get { return sfxVolume; }
    }
    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField, Range(0, 1)]
    float ambianceVolume;
    public float AmbianceVolume
    {
        get { return ambianceVolume; }
    }
    [SerializeField]
    private AudioSource ambianceSource;

    [SerializeField, Range(0, 1)]
    float dialogueVolume;
    public float DialogueVolume
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

    public void UpdateAllVolumeValues(float _masterVolume, float _musicVolume, float _sfxVolume, float _ambianceVolume, float _dialogueVolume)
    {
        masterVolume = _masterVolume;
        musicVolume = _masterVolume * _musicVolume;
        sfxVolume = _masterVolume * _sfxVolume;
        ambianceVolume = _masterVolume * _ambianceVolume;
        dialogueVolume = _masterVolume * _dialogueVolume;
        ResetAllSound();
    }

    private void ResetAllSound()
    {
        Destroy(musicSource);
        Destroy(sfxSource);
        Destroy(dialogueSource);
        Destroy(ambianceSource);

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        dialogueSource = gameObject.AddComponent<AudioSource>();
        ambianceSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartMusic(string musicType)
    {
        StopMusic();
        shouldPlayMusic = true;
        System.Random rnd = new();

        AudioClip[] allClips = SoundList[(int)Enum.Parse(typeof(SoundType), musicType)].Sounds;
        musicSource.clip = allClips[rnd.Next(0, allClips.Length)];
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void UnPauseMusic()
    {
        musicSource.UnPause();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void StartSFX(string sfxType, GameObject player = null, GameObject audioObject = null)
    {
        System.Random rnd = new();

        AudioClip[] allClips = SoundList[(int)Enum.Parse(typeof(SoundType), sfxType)].Sounds;
        sfxSource.clip = ;
        sfxSource.PlayOneShot(allClips[rnd.Next(0, allClips.Length)], sfxVolume);

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
    public AudioClip[] Sounds
    {
        get { return sounds; }
    }
}
