using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Load scene
    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
    }

    public void Play()
    {
        audioManager.partOfGame++;
        audioManager.StartSFX(SoundType.BellSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Quit game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }

    public void CreditsAudio()
    {
        audioManager.StartSFX(SoundType.CutlerySound);
        audioManager.StartSFX(SoundType.CookieSound);
    }

    public void SettingsAudio()
    {
        audioManager.StartSFX(SoundType.PotSound);
    }
}

