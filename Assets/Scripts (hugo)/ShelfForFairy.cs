using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfForFairy : MonoBehaviour, IInteractable
{
    public FlyAway cubeToActivate; // Reference to Cube B (the flying cube)

    public void Interact()
    {
        if (cubeToActivate != null)
        {
            cubeToActivate.StartFlying();
            Debug.Log("Activated Cube B to start flying.");
        }
    }
}
