using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrossingLine : MonoBehaviour
{
    public GameObject movingObject;      // The object that moves
    public float speed = 5f;            // Speed of the object
    public float crossingLineX = 1f;   // X position of the crossing line
    public float detectionRange = 1f;  // How close to the line the object needs to be to detect a crossing
    public TextMeshProUGUI counterText; // UI element to display the counter

    private int counter = 0;  // Counter for successful clicks

    void Start()
    {
        UpdateCounterText();
    }

    void Update()
    {
        CheckForCrossing();
    }

    void CheckForCrossing()
    {
        if (Mathf.Abs(movingObject.transform.position.x - crossingLineX) < detectionRange)
        {
            if (Input.GetMouseButtonDown(0))
            {
                counter++;
                UpdateCounterText();
            }
        }
    }

    // Function to update the UI text with the current counter value
    void UpdateCounterText()
    {
        counterText.text = "Items: " + counter; // Changed "Count" to "Items"
    }
}
