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

    AudioManager audioManager;

    bool active = false;
    Vector3 prevCamPos;
    Quaternion prevCamRot;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
    }

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
            Debug.Log("closing the book");
            active = false;
            player.SetActive(true);
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
            player.SetActive(false);
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

    public void ResetRecipes()
    {
        foreach (Transform item in cookbookUI.transform.GetChild(3).transform)
        {
            item.gameObject.SetActive(false);
        }
        cookbookUI.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
        cookbookUI.transform.GetChild(3).transform.GetChild(1).gameObject.SetActive(true);
    }

    public void TurnPageAudio()
    {
        if (audioManager != null)
        {
            audioManager.StartSFX(SoundType.PageSound);
        }
    }
}
