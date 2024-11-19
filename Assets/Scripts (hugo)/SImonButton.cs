using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SImonButton : MonoBehaviour
{
    public int buttonIndex;
    private Simon simonGame;

    private void Start()
    {
        simonGame = FindObjectOfType<Simon>();
    }

    private void OnMouseDown()
    {
        simonGame.ButtonPressed(buttonIndex);
    }
}
