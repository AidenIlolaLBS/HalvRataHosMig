using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameActivate : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject Camera;
    [SerializeField] GameObject crosshair;

    public void Interact()
    {
        panel.SetActive(true);
        crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disables movement
        player.GetComponent<PlayerMove>().enabled = false;
        Camera.GetComponent<PlayerCam>().enabled = false;
    }
}
