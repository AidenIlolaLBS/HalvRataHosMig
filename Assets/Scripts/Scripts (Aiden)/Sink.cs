using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    public GameObject water;
    private bool waterActive = false;
    private void Start()
    {
        water.SetActive(waterActive);
    }
    public void Interact()
    {
        if (waterActive)
        {
            waterActive = false;            
        }
        else
        {
            waterActive = true;
        }
        water.SetActive(waterActive);
    }
    public bool GetWaterStatus()
    {
        return waterActive;
    }
}
