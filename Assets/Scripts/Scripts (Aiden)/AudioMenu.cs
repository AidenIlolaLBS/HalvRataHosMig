using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambianceSlider;
    public Slider dialogueSlider;

    AudioManager audioManager;

    float time = 0;
    float maxWait = 0.5f;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > maxWait)
        {
            time = 0;
            UpdateSoundVolume();
        }
    }

    public void UpdateSoundVolume()
    {
        audioManager.UpdateAllVolumeValues(masterSlider.value, musicSlider.value, sfxSlider.value, ambianceSlider.value, dialogueSlider.value);
        DisplayCurrentVolume();
    }

    public void DisplayCurrentVolume()
    {
        masterSlider.value = (float)audioManager.MasterVolume;
        musicSlider.value = (float)(audioManager.MusicVolume / audioManager.MasterVolume);
        sfxSlider.value = (float)(audioManager.SfxVolume / audioManager.MasterVolume);
        ambianceSlider.value = (float)(audioManager.AmbianceVolume / audioManager.MasterVolume);
        dialogueSlider.value = (float)(audioManager.DialogueVolume / audioManager.MasterVolume);
    }
}
