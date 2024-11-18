using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class ScoreMeter : MonoBehaviour
{
    public Slider scoreSlider;
    public Image fillImage;
    public Color positiveColor = Color.green;
    public Color negativeColor = Color.red;
    private float score = 0;
    void Start()
    {
        UpdateMeter();
    }

    public void ChangeScore(float value)
    {
        score += value;
        score = Mathf.Clamp(score, scoreSlider.minValue, scoreSlider.maxValue);
        UpdateMeter();
    }

    private void UpdateMeter()
    {
        if (score >= 0)
        {
            fillImage.color = positiveColor;
            Debug.Log("Color is set to positive");
        }
        else
        {
            fillImage.color = negativeColor;
            Debug.Log("Color is set to negative");
        }
        scoreSlider.value = score;
        
    }
}

