using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    public GameObject water;
    public GameObject handle;
    private bool waterActive = false;
    Quaternion targetRotaionZ = new();
    public const float rotationSpeed = 1;

    private void Start()
    {
        water.SetActive(waterActive);
        targetRotaionZ = handle.transform.rotation;
    }

    private void Update()
    {
        handle.transform.rotation = Quaternion.Lerp(handle.transform.rotation, targetRotaionZ, Time.deltaTime * rotationSpeed);
    }

    public void Interact()
    {
        if (waterActive)
        {
            waterActive = false;
            targetRotaionZ = new Quaternion(handle.transform.rotation.x, handle.transform.rotation.y, -90,0);
        }
        else
        {
            waterActive = true;
            targetRotaionZ = new Quaternion(handle.transform.rotation.x, handle.transform.rotation.y, 90, 0);
        }
        water.SetActive(waterActive);
    }
    public bool GetWaterStatus()
    {
        return waterActive;
    }
}
