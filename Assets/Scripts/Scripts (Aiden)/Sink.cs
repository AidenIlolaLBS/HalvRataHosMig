using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    public GameObject water;
    public GameObject handle;
    private bool waterActive = false;
    Quaternion targetRotaionZ = new();
    const float rotationSpeed = 180;

    private void Start()
    {
        water.SetActive(waterActive);
        //targetRotaionZ = handle.transform.rotation;
    }

    private void Update()
    {
        //handle.transform.rotation = Quaternion.RotateTowards(handle.transform.rotation, targetRotaionZ, Time.deltaTime * rotationSpeed);
    }

    public void Interact()
    {
        if (waterActive)
        {
            waterActive = false;
            //targetRotaionZ = Quaternion.Euler(handle.transform.rotation.x, handle.transform.rotation.y, 0);
        }
        else
        {
            waterActive = true;
            //targetRotaionZ = Quaternion.Euler(handle.transform.rotation.x, handle.transform.rotation.y, -180);
        }
        water.SetActive(waterActive);
    }
    public bool GetWaterStatus()
    {
        return waterActive;
    }
}
