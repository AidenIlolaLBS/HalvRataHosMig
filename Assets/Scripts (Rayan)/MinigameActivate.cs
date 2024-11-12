using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameActivate : MonoBehaviour, IInteractable
{
    public GameObject panel;
    public GameObject player;
    public GameObject Camera;

    public void Interact()
    {
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disables movement
        player.GetComponent<PlayerMove>().enabled = false;
        Camera.GetComponent<PlayerCam>().enabled = false;
    }
}
