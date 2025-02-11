using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public TMP_Text text;
    double currentTime = 100;

    // Start is called before the first frame update
    void Start()
    {
        text.text = currentTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        text.text = Math.Round(currentTime, 0).ToString();
    }
}
