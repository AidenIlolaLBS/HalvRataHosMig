using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Focus : MonoBehaviour
{
    public GameObject effects;

    bool activeDOF = false;

    private void Awake()
    {
        effects.SetActive(activeDOF);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (activeDOF)
            {
                activeDOF = false;
                effects.SetActive(activeDOF);
            }
            else
            {
                activeDOF = true;
                effects.SetActive(activeDOF);
            }
        }
    }
}
