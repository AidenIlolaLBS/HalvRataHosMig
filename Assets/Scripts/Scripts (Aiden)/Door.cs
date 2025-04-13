using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool fridge = false;
    public bool switchScene = false;
    bool open = false;
    public bool Open
    {
        get { return open; }
    }
    public float doorAngle = 90;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
    }

    public void InteractDoor()
    {
        if (open)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            open = false;
            if (fridge)
            {
                audioManager.StartSFX(SoundType.FridgeDoorCloseSound);
            }
            else
            {
                audioManager.StartSFX(SoundType.CloseDoorSound);
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.y - doorAngle, 0);
            open = true;
            if (fridge)
            {
                audioManager.StartSFX(SoundType.FridgeDoorOpenSound);
            }
            else
            {
                audioManager.StartSFX(SoundType.OpenDoorSound);
            }
        }

        if (switchScene)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSceneManager>().NextScene();
            audioManager.StartSFX(SoundType.CloseDoorSound);
        }
    }
}
