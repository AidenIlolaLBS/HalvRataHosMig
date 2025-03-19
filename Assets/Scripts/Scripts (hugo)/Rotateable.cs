using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateable : MonoBehaviour
{
    public float rotationStep = 90f; // How much the image rotates per press
    private float currentRotation = 0f; // Tracks the current rotation
    public float correctRotation = 0f; // The correct orientation for this image
    public Transform linkedImage; // Reference to the linked image's Transform

    void Start()
    {
        SetRandomRotations();
    }

    private void SetRandomRotations()
    {
        // Set the starting rotation to a random multiple of the rotation step
        currentRotation = rotationStep * Random.Range(0, 360 / (int)rotationStep);
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);

        // Set the correct rotation to another random multiple of the rotation step
        correctRotation = rotationStep * Random.Range(0, 360 / (int)rotationStep);

        // If there is a linked image, synchronize its correct rotation with this one
        if (linkedImage != null)
        {
            linkedImage.rotation = Quaternion.Euler(0f, 0f, correctRotation);
        }

        Debug.Log($"Initial rotation: {currentRotation}, Target rotation: {correctRotation}");
    }


    // Rotate the image clockwise
    public void RotateClockwise()
    {
        currentRotation += rotationStep;
        if (currentRotation >= 360f)
        {
            currentRotation -= 360f; // Loop back around
        }

        UpdateRotation();
    }

    // Rotate the image counterclockwise
    public void RotateCounterclockwise()
    {

        currentRotation -= rotationStep;
        if (currentRotation < 0f)
        {
            currentRotation += 360f; // Loop back around
        }

        UpdateRotation();
    }

    // Apply the rotation and check if it's correct
    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        // Check if this image is correctly oriented
        if (Mathf.Approximately(currentRotation, correctRotation))
        {
            Debug.Log(gameObject.name + " is correctly oriented!");
        }
    }
}