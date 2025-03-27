using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookBook : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCamera;
    public GameObject cookbookUI;
    public GameObject camCookBook;
    public MoveCamera moveCamera;

    bool active = false;
    Vector3 prevCamPos;
    Quaternion prevCamRot;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && active)
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (active)
        {
            active = false;
            player.GetComponent<PlayerMove>().enabled = true;
            playerCamera.GetComponent<Interact>().enabled = true;
            playerCamera.GetComponent<PickUpScript>().enabled = true;
            playerCamera.GetComponent<PlayerCam>().enabled = true;
            playerCamera.transform.position = prevCamPos;
            playerCamera.transform.rotation = prevCamRot;
            moveCamera.enabled = true;
            cookbookUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            active = true;
            player.GetComponent<PlayerMove>().enabled = false;
            playerCamera.GetComponent<Interact>().enabled = false;
            playerCamera.GetComponent<PickUpScript>().enabled = false;
            playerCamera.GetComponent<PlayerCam>().enabled = false;
            prevCamPos = playerCamera.transform.position;
            prevCamRot = playerCamera.transform.rotation;
            playerCamera.transform.position = camCookBook.transform.position;
            playerCamera.transform.rotation = camCookBook.transform.rotation;
            moveCamera.enabled = false;
            cookbookUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
