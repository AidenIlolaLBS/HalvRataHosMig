using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnMove : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject Camera;

    public void Start()
    {
        // Returns all abilitys that may be lost
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player.GetComponent<PlayerMove>().enabled = true;
        Camera.GetComponent<PlayerCam>().enabled = true;
    }
}
