using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DOFPersons : MonoBehaviour
{
    Camera[] cameras = new Camera[2];
    public GameObject player;
    GameObject person;
    public float rotationSpeed;
    public float zoomSpeed;
    public float zoomIn;
    float startZoom;

    bool shouldRotate = false;
    bool shouldZoom = false;

    // Start is called before the first frame update
    void Start()
    {
        cameras = Camera.allCameras;
        startZoom = cameras[0].fieldOfView;
    }

    private void Update()
    {
        if (shouldRotate)
        {
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, person.transform.rotation, Time.deltaTime * rotationSpeed);
            if (player.transform.rotation == person.transform.rotation)
            {
                shouldRotate = false;
            }
        }

        if (shouldZoom)
        {
            cameras[0].fieldOfView = Mathf.Lerp(cameras[0].fieldOfView, zoomIn, Time.deltaTime * zoomSpeed);
            cameras[1].fieldOfView = Mathf.Lerp(cameras[1].fieldOfView, zoomIn, Time.deltaTime * zoomSpeed);
        }
    }

    public void TargetObject (GameObject gameObject)
    {
        person = gameObject;
        person.layer = LayerMask.NameToLayer("NoDepthOfField");

        shouldRotate = true;
    }
}
