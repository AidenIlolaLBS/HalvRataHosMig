using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOFPersons : MonoBehaviour
{
    Camera[] cameras = new Camera[2];
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        cameras = Camera.allCameras;
    }

    public void TargetObject (GameObject gameObject)
    {

    }
}
