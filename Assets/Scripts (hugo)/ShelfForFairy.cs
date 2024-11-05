using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfForFairy : MonoBehaviour, IInteractable
{
    public FlyAway[] cubesToActivate; // Array to hold multiple cubes to activate

    public void Interact()
    {
        foreach (FlyAway cube in cubesToActivate)
        {
            if (cube != null)
            {
                cube.StartFlying();
                Debug.Log("Activated a cube to start flying.");
            }
        }
    }
}
