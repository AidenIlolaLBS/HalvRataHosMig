using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simon : MonoBehaviour
{
    public GameObject[] buttons; // Array of color buttons
    public float displayDelay = 1f; // Time between color displays

    private List<int> sequence = new List<int>(); // Holds the Simon sequence
    private int playerIndex = 0; // Tracks the player’s position in the sequence
    private bool isPlayerTurn = false;

    void Start()
    {
        StartNewRound();
    }

    void StartNewRound()
    {
        sequence.Clear(); // Clear the previous round's sequence

        // Add exactly 4 random colors to the sequence
        for (int i = 0; i < 4; i++)
        {
            sequence.Add(Random.Range(0, buttons.Length));
        }

        StartCoroutine(DisplaySequence());
    }


    private IEnumerator DisplaySequence()
    {
        isPlayerTurn = false;

        for (int i = 0; i < sequence.Count; i++)
        {
            int buttonIndex = sequence[i];
            HighlightButton(buttonIndex);
            yield return new WaitForSeconds(displayDelay);
            ResetButton(buttonIndex);
            yield return new WaitForSeconds(0.5f);
        }

        isPlayerTurn = true;
        playerIndex = 0;
    }

    private void HighlightButton(int index)
    {
        // Change the button color to white or another highlight color
        buttons[index].GetComponent<Image>().color = Color.white; // example highlight
    }

    private void ResetButton(int index)
    {
        // Reset button to its original color
        switch (index)
        {
            case 0: buttons[index].GetComponent<Image>().color = Color.yellow; break;
            case 1: buttons[index].GetComponent<Image>().color = Color.green; break;
            case 2: buttons[index].GetComponent<Image>().color = Color.blue; break;
            case 3: buttons[index].GetComponent<Image>().color = Color.red; break;
        }
    }

    public void ButtonPressed(int buttonIndex)
    {
        if (!isPlayerTurn) return;

        if (sequence[playerIndex] == buttonIndex)
        {
            playerIndex++;
            if (playerIndex >= sequence.Count)
            {
                Debug.Log("Correct Sequence! Starting New Round...");
                StartNewRound();
            }
        }
        else
        {
            Debug.Log("Incorrect Sequence! Game Over.");
            ResetGame();
        }
    }

    private void ResetGame()
    {
        sequence.Clear();
        StartNewRound();
    }
}
