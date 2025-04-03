using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TestFocus : MonoBehaviour
{
    public Camera mainCam;
    bool activeDOF = false;
    float startDOF;

    private void Awake()
    {
        startDOF = mainCam.focusDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (activeDOF)
            {
                mainCam.focusDistance = startDOF;
                activeDOF = false;
                Debug.Log(startDOF);
            }
            else
            {
                mainCam.focusDistance = 1;
                activeDOF = true;
                Debug.Log(startDOF);
            }
        }
    }
}
