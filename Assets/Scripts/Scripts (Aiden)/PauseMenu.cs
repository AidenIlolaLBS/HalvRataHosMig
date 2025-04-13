using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // Update is called once per frame

    GameObject playerCam;
    GameObject player;

    private void Start()
    {
        DontDestroyOnLoad(this);        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                playerCam = GameObject.FindGameObjectWithTag("MainCamera");
                player = GameObject.FindGameObjectWithTag("Player");
                if (!pauseMenu.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    playerCam.transform.parent.GetComponent<MoveCamera>().enabled = false;
                    playerCam.GetComponent<Interact>().enabled = false;
                    playerCam.GetComponent<PickUpScript>().enabled = false;
                    playerCam.GetComponent<PlayerCam>().enabled = false;

                    player.GetComponent<PlayerMove>().enabled = false;

                    pauseMenu.SetActive(true);
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    playerCam.transform.parent.GetComponent<MoveCamera>().enabled = true;
                    playerCam.GetComponent<Interact>().enabled = true;
                    playerCam.GetComponent<PickUpScript>().enabled = true;
                    playerCam.GetComponent<PlayerCam>().enabled = true;

                    player.GetComponent<PlayerMove>().enabled = true;

                    pauseMenu.transform.GetChild(0).gameObject.SetActive(true);
                    pauseMenu.transform.GetChild(1).gameObject.SetActive(false);

                    pauseMenu.SetActive(false);
                }
            }
        }
    }

    public void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCam.transform.parent.GetComponent<MoveCamera>().enabled = true;
        playerCam.GetComponent<Interact>().enabled = true;
        playerCam.GetComponent<PickUpScript>().enabled = true;
        playerCam.GetComponent<PlayerCam>().enabled = true;

        player.GetComponent<PlayerMove>().enabled = true;

        pauseMenu.SetActive(false);
    }

    public void ButtonSound()
    {
        //gameObject.GetComponent<AudioManager>().StartSFX(SoundType.ButtonSound);
    }

    public void RestartGame()
    {
        Application.Quit();
    }
}
