using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum SoundType
{
    MenuMusic,
    KitchenMusic,
    EatingMusic,
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
    Ambiance,
    ClockSound,
    PageSound,
    CookieSound,
    CutlerySound,
    PotSound,
    BellSound,
    LikeSound,
    DislikeSound,
    SuccessSound,
    HeartBeatSound,
    TapSound,
    FridgeDoorOpenSound,
    FridgeDoorCloseSound,
    FairySound,
    SpiderSound,
    WinSound,
    LoseSound
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundList[] SoundList;

    /// <summary>
    /// 0: main menu / pause menu. 1: in kitchen. 2: eating food.
    /// </summary>
    public int partOfGame = 0;

    [SerializeField, Range(0, 1)]
    float masterVolume;
    public float MasterVolume
    {
        get { return masterVolume; }
    }

    [SerializeField, Range(0, 1)]
    float musicVolume;
    public float musicSliderValue;
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
    public float sfxSliderValue;
    public float SfxVolume
    {
        get { return sfxVolume; }
    }
    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField, Range(0, 1)]
    float ambianceVolume;
    public float ambianceSliderValue;
    public float AmbianceVolume
    {
        get { return ambianceVolume; }
    }
    [SerializeField]
    private AudioSource ambianceSource;

    [SerializeField, Range(0, 1)]
    float dialogueVolume;
    public float dialogueSliderValue;
    public float DialogueVolume
    {
        get { return dialogueVolume; }
    }
    [SerializeField]
    private AudioSource dialogueSource;

    private bool startCodeRun = false;

    void Awake()
    {
        if (Application.isPlaying)
        {
            musicSliderValue = musicVolume;
            ambianceSliderValue = ambianceVolume;
            sfxSliderValue = sfxVolume;
            dialogueSliderValue = dialogueVolume;

            player = GameObject.FindGameObjectWithTag("Player");

            startCodeRun = true;
            DontDestroyOnLoad(gameObject);
        }
    }

    int musicPlaying = 0;
    GameObject sfxAudioObject;
    GameObject player;

    private void Update()
    {
        if (Application.isPlaying)
        {
            if (!startCodeRun)
            {
                musicSliderValue = musicVolume;
                ambianceSliderValue = ambianceVolume;
                sfxSliderValue = sfxVolume;
                dialogueSliderValue = dialogueVolume;

                player = GameObject.FindGameObjectWithTag("Player");

                startCodeRun = true;
                DontDestroyOnLoad(gameObject);
            }
            if (!musicSource.isPlaying && shouldPlayMusic || musicPlaying != partOfGame)
            {
                switch (partOfGame)
                {
                    case 0:
                        StartMusic(SoundType.MenuMusic);
                        musicPlaying = 0;
                        break;
                    case 1:
                        StartMusic(SoundType.KitchenMusic);
                        musicPlaying = 1;
                        break;
                    case 2:
                        StartMusic(SoundType.EatingMusic);
                        musicPlaying = 2;
                        break;
                    default:
                        break;
                }
            }

            if (sfxSource.isPlaying && player != null && sfxAudioObject != null)
            {
                float distance = (float)(Math.Pow(player.transform.position.x - sfxAudioObject.transform.position.x, 2) + Math.Pow(player.transform.position.z - sfxAudioObject.transform.position.z, 2));
                sfxSource.volume = sfxVolume / distance;
            }
        }      
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

    public void StopSFX()
    {
        Destroy(sfxSource);
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void StartMusic(SoundType soundType)
    {
        StopMusic();
        shouldPlayMusic = true;
        System.Random rnd = new();

        AudioClip[] allClips = SoundList[(int)soundType].Sounds;
        if (allClips.Length > 0)
        {
            musicSource.clip = allClips[rnd.Next(0, allClips.Length)];
            musicSource.volume = musicVolume;
            musicSource.Play();
        }       
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

    public float StartSFX(SoundType soundType, bool oneShot = true, bool loop = false, GameObject player = null, GameObject audioObject = null)
    {
        System.Random rnd = new();

        AudioClip[] allClips = SoundList[(int)soundType].Sounds;
        float distance = 1;

        int clipIndex = rnd.Next(0, allClips.Length);

        if (player != null && audioObject != null)
        {
            distance = (float)(Math.Pow(player.transform.position.x - audioObject.transform.position.x, 2) + Math.Pow(player.transform.position.z - audioObject.transform.position.z, 2));
        }

        if (oneShot)
        {
            sfxSource.PlayOneShot(allClips[clipIndex], sfxVolume / distance);
        }
        else
        {
            sfxSource.clip = allClips[clipIndex];
            sfxSource.volume = sfxVolume / distance;
            sfxSource.loop = loop;
        }

        return allClips[clipIndex].length;
    }

    public void StartSFX(AudioClip audioClip, bool oneShot = true, GameObject player = null, GameObject audioObject = null)
    {

        float distance = 1;
        if (player != null && audioObject != null)
        {
            distance = (float)(Math.Pow(player.transform.position.x - audioObject.transform.position.x, 2) + Math.Pow(player.transform.position.z - audioObject.transform.position.z, 2));
        }
        if (oneShot)
        {
            sfxSource.PlayOneShot(audioClip, sfxVolume / distance);
        }
        else
        {
            sfxSource.clip = audioClip;
            sfxSource.volume = sfxVolume / distance;
        }
    }

    public void StartDialogue(SoundType soundType)
    {
        System.Random rnd = new();

        AudioClip[] allClips = SoundList[(int)soundType].Sounds;
        if (allClips.Length > 0)
        {
            dialogueSource.clip = allClips[rnd.Next(0, allClips.Length)];
            dialogueSource.volume = dialogueVolume;
            dialogueSource.Play();
        }
    }
    public void StopDialogue()
    {
        dialogueSource.Stop();
    }

    public void StartAmbiance()
    {
        System.Random rnd = new();

        AudioClip[] allClips = SoundList[(int)SoundType.Ambiance].Sounds;
        if (allClips.Length > 0)
        {
            ambianceSource.clip = allClips[rnd.Next(0, allClips.Length)];
            ambianceSource.volume = ambianceVolume;
            ambianceSource.Play();
        }
    }
    public void StopAmbiance()
    {
        ambianceSource.Stop();
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
